import serial

import time
import threading
import serial.tools.list_ports
import json
import os

blacklist = ["/dev/cu.wlan-debug", "/dev/cu.MovsMacBookAir", "/dev/cu.Bluetooth-Incoming-Port"]
ports = [port.device for port in serial.tools.list_ports.comports() if port.device not in blacklist]
ports.sort(reverse=True)

# 全局变量，用于存储从串口读取的数据
data_from_port1 = ""
data_from_port2 = ""

# 全局字典，用于存储解析后的数据
knob_data = {}
mpu_data = {}
previous_knob_data = {}

# 全局变量，用于存储模式和角度
knob_mode = 0
previous_knob_mode = 0
initial_angle1 = None
initial_angle2 = None
initial_angle3 = None
vel_lst = [0] * 10

# 锁，用于同步对全局变量的操作
lock = threading.Lock()

button = 0
timer = 0


def parse_knob_data(data):
    global knob_mode,button
    """Parse the knob data from the received string."""
    parsed_data = {}
    try:
        parts = data.split(',')
        for part in parts:
            if ':' in part:
                key, value = part.split(':')
                parsed_data[key.strip()] = float(value.strip())
         # 检查是否包含指定的中文词组并更新模式


        if "有界顺滑模式" in data:
            knob_mode = 1
        elif "有界棘轮模式 2" in data:
            knob_mode = 2.2
        elif "有界棘轮模式 3" in data:
            knob_mode = 2.3
        elif "有界棘轮模式 4" in data:
            knob_mode = 2.4
        elif "有界棘轮模式 5" in data:
            knob_mode = 2.5
        elif "无界顺滑模式" in data:
            knob_mode = 3
        elif "BUTTONDOWN" in data:
            button = 1

    except Exception as e:
        print(f"Error parsing knob data: {e}")
    return parsed_data

def parse_mpu_data(data):
    """Parse the MPU data from the received string."""

    parsed_data = {}
    try:
        parts = data.split(',')
        for part in parts:
            if ':' in part:
                key, value = part.split(':')
                parsed_data[key.strip()] = float(value.strip())


    except Exception as e:
        print(f"Error parsing MPU data: {e}")
    return parsed_data


def print_data():
    """Continuously print the latest data from both ports."""
    global  timer,button,data_from_port1, data_from_port2, knob_mode,previous_knob_mode,previous_knob_data
    while True:
        with lock:
            print(f"KNOB: {knob_data}")
            print(f"MPU: {mpu_data}")
            print(f"KNOB_MODE: {knob_mode}")
            print(f"CURRENT BAR (SPRING): {current_bar_spring()}")
            print(f"CURRENT TRIGGER (SPRING): {current_trigger_spring()}")
            print(f"CURRENT SELECTION:{current_selection()}")
            print(f"CURRENT BAR (SMOOTH): {current_bar_smooth()}")
            print(f"BUTTON: {button}")
            print("--------------")
            previous_knob_mode=knob_mode
            previous_knob_data=knob_data

            # 将数据写入 JSON 文件
            data = {
                # **knob_data,  # 将 knob_data 字典的内容提取出来
                **mpu_data,  # 将 mpu_data 字典的内容提取出来
                'knob_mode': knob_mode,
                'knob_bar_spring': current_bar_spring(),
                'knob_trigger_spring': current_trigger_spring(),
                'knob_selection': current_selection(),
                'knob_bar_smooth': current_bar_smooth(),
                'Trigger': button
            }
            with open('data.json', 'w') as f:
                json.dump(data, f)
        time.sleep(0.1)  # 每秒打印一次
        if button == 1:
            timer += 0.1
            if timer >= 0.5:
                button = 0
                timer = 0
        else:
            timer = 0


def current_bar_spring():
    global knob_mode, initial_angle1, knob_data,previous_knob_mode
    #print(f"PREV_KNOB_MODE: {previous_knob_mode}")
    #print(f"INITIAL ANGLE:{initial_angle1}")

    current_angle=0
    if knob_data:
        current_angle = knob_data.get("knob_angle", 0)
    else:
        current_angle=previous_knob_data.get("knob_angle", 0)

    # 检查当前模式
    if previous_knob_mode != 1 and knob_mode == 1 and initial_angle1 is None:   # 获取当前角度
        # 第一次进入模式1，记录当前角度为初始角度
            initial_angle1 = current_angle

    if previous_knob_mode == 1 and knob_mode == 1 and initial_angle1 is not None:   # 获取当前角度
        # 计算当前角度减去初始角度
        return round(-(current_angle - initial_angle1)*100,2)
    if previous_knob_mode == 1 and knob_mode != 1 and initial_angle1 is not None:   # 获取当前角度
        initial_angle1 = None

    return 0


def current_bar_spring():
    global knob_mode, initial_angle1, knob_data, previous_knob_mode
    # print(f"PREV_KNOB_MODE: {previous_knob_mode}")
    # print(f"INITIAL ANGLE:{initial_angle1}")

    current_angle = 0
    if knob_data:
        current_angle = knob_data.get("Current Angle", 0)
    else:
        current_angle = previous_knob_data.get("Current Angle", 0)

    # 检查当前模式
    if previous_knob_mode != 1 and knob_mode == 1 and initial_angle1 is None:  # 获取当前角度
        # 第一次进入模式1，记录当前角度为初始角度
        initial_angle1 = current_angle

    if previous_knob_mode == 1 and knob_mode == 1 and initial_angle1 is not None:  # 获取当前角度
        # 计算当前角度减去初始角度
        return round(-(current_angle - initial_angle1) * 100, 2)
    if previous_knob_mode == 1 and knob_mode != 1 and initial_angle1 is not None:  # 获取当前角度
        initial_angle1 = None

    return 0


def current_trigger_spring():
    global knob_data, vel_lst, knob_mode
    if knob_mode == 1:
        vel = knob_data.get("Current Velocity", 0)

        threshold = 8
        confi_interval = 2

        # 创建一个固定大小的队列，用于存储最近 0.5 秒的数据
        def append_and_shift(lst, value):
            # 将新值添加到列表末尾
            lst.append(value)
            # 删除列表的第一个元素，保持列表长度为 10
            lst.pop(0)
            return lst

        vel_lst = append_and_shift(vel_lst, vel)

        vel_max = max(vel_lst)
        index_max = vel_lst.index(vel_max)
        if index_max in range(0, 8) and vel_max >= threshold:
            pre_vel_max = vel_lst[index_max - 1] - confi_interval
            after_vel_max = vel_lst[index_max + 1] + confi_interval
            if pre_vel_max * after_vel_max < 0:
                return 1
        else:
            return 0


def current_bar_smooth():
    global knob_mode, initial_angle3, knob_data, previous_knob_mode
    # print(f"PREV_KNOB_MODE: {previous_knob_mode}")
    # print(f"111KNOB_MODE: {knob_mode}")
    # print(f"INITIAL ANGLE:{initial_angle3}")

    current_angle = 0
    if knob_data:
        current_angle = knob_data.get("Current Angle", 0)
    else:
        current_angle = previous_knob_data.get("Current Angle", 0)

    # 检查当前模式
    if previous_knob_mode != 3 and knob_mode == 3 and initial_angle3 is None:  # 获取当前角度
        # 第一次进入模式1，记录当前角度为初始角度
        initial_angle3 = current_angle

    if previous_knob_mode == 3 and knob_mode == 3 and initial_angle3 is not None:  # 获取当前角度
        # 计算当前角度减去初始角度
        return round(-(current_angle - initial_angle3) * 100, 2)
    if previous_knob_mode == 3 and knob_mode != 3 and initial_angle3 is not None:  # 获取当前角度
        initial_angle3 = None

    return 0


def current_selection():
    global knob_mode, initial_angle2, knob_data, previous_knob_mode
    current_angle = 0
    num_ratchet = 0
    if knob_data:
        current_angle = knob_data.get("Current Angle", 0)
    else:
        current_angle = previous_knob_data.get("Current Angle", 0)

        # 检查当前模式
    if previous_knob_mode not in {2.2, 2.3, 2.4, 2.5} and knob_mode in {2.2, 2.3, 2.4,
                                                                        2.5} and initial_angle2 is None:  # 获取当前角度
        # 第一次进入模式1，记录当前角度为初始角度
        initial_angle2 = current_angle

    if previous_knob_mode in {2.2, 2.3, 2.4, 2.5} and knob_mode in {2.2, 2.3, 2.4,
                                                                    2.5} and initial_angle2 is not None:  # 获取当前角度

        num_ratchet = int(str(knob_mode).split('.')[1])

        # 定义区间
        lower_bound = initial_angle2 - (90 / 2) * 3.1415926 / 180
        upper_bound = initial_angle2 + (90 / 2) * 3.1415926 / 180

        # 计算区间长度
        interval_length = upper_bound - lower_bound

        # 计算每个子区间的长度
        sub_interval_length = interval_length / num_ratchet

        # 找到 current_angle 落在第几个区间
        for i in range(num_ratchet):

            if lower_bound + i * sub_interval_length <= current_angle < lower_bound + (i + 1) * sub_interval_length:
                return i + 1
            if lower_bound > current_angle:
                return 1
            if upper_bound < current_angle:
                return num_ratchet

        # 如果 current_angle 恰好是 upper_bound，返回最后一个区间
        if current_angle == upper_bound:
            return num_ratchet

    if previous_knob_mode in {2.2, 2.3, 2.4, 2.5} and knob_mode not in {2.2, 2.3, 2.4,
                                                                        2.5} and initial_angle2 is not None:  # 获取当前角度
        initial_angle2 = None

    return 0



def read_from_port(serial_port, port_number):
    """Continuously read data from the serial port and store it in the provided storage."""
    global data_from_port1, data_from_port2, knob_data, mpu_data
    while True:
        if serial_port.in_waiting > 0:
            data = serial_port.readline().decode('utf-8', errors='ignore').strip()
            # 使用锁来同步对全局变量的操作
            with lock:
                if port_number == 2:
                    data_from_port1 = data
                    knob_data = parse_knob_data(data)
                elif port_number == 1:
                    data_from_port2 = data
                    mpu_data = parse_mpu_data(data)


def main():
    global port1, port2

    port2 = '/dev/cu.wchusbserial11330'  # Replace with your actual serial port
    port1 = '/dev/cu.wchusbserial11320'  # Replace with your actual serial port
    baud_rate1 = 9600
    baud_rate2 = 115200

    # Open the serial ports
    serial_port1 = serial.Serial(port1, baud_rate1, timeout=1)
    serial_port2 = serial.Serial(port2, baud_rate2, timeout=1)

    # Create threads to read from each port
    thread1 = threading.Thread(target=read_from_port, args=(serial_port1, 1))
    thread2 = threading.Thread(target=read_from_port, args=(serial_port2, 2))
    thread3 = threading.Thread(target=print_data)

    # Start the threads
    thread1.start()
    thread2.start()
    thread3.start()

    # Wait for the threads to finish (they won't, since they run forever)
    thread1.join()
    thread2.join()
    thread3.join()


if __name__ == '__main__':
    main()