import serial
import serial.tools.list_ports
import json
import os

def validate_data(line):
    parts = line.split(',')
    return len(parts) == 15

def read_from_port(ser):
    while True:
        try:
            line = ser.readline().decode().strip()
            if validate_data(line):
                parts = line.split(',')
                data = {
                    'x': float(parts[0]),
                    'y': float(parts[1]),
                    'z': float(parts[2]),
                    'Trigger': float(parts[3]),
                    'Menu': float(parts[4]),
                    'Down': float(parts[5]),
                    'Up': float(parts[6]),
                    'Grip': float(parts[7]),
                    'acc_x': float(parts[8]),
                    'acc_y': float(parts[9]),
                    'acc_z': float(parts[10]),
                    'temp': float(parts[11]),
                    'gyr_x': float(parts[12]),
                    'gyr_y': float(parts[13]),
                    'gyr_z': float(parts[14])
                }
                print(json.dumps(data))
                with open('data.json', 'w') as f:
                    json.dump(data, f)
                send_json_to_port(ser, 'data_out.json')  # 修改这一行
            else:
                print("Invalid data received: ", line)
        except KeyboardInterrupt:
            break

def send_json_to_port(ser, json_file):
    if os.path.getsize(json_file) > 0:  # 检查文件是否为空
        with open(json_file, 'r') as f:
            try:
                data = json.load(f)
            except json.JSONDecodeError:
                print(f"Invalid JSON in {json_file}. Skipping...")
                return

            if len(data) == 3:  # 检查数据的长度
                ser.write((json.dumps(data) + '\n').encode())  # 在数据后添加换行符
                print(f"Sent data to {ser.name}， data: {json.dumps(data)}")
            else:
                print(f"Data in {json_file} does not have 3 items. Skipping...")
    else:
        print(f"{json_file} is empty. Skipping...")

ports = serial.tools.list_ports.comports()

# 黑名单串口
blacklist = ["/dev/cu.wlan-debug", "/dev/cu.MovsMacBookAir", "/dev/cu.Bluetooth-Incoming-Port"]

# 在主循环中
for port in ports:
    # 如果串口在黑名单中，跳过
    if port.device in blacklist:
        continue
    try:
        ser = serial.Serial(port.device, 9600)
        if ser.is_open:
            line = ser.readline().decode().strip()
            if validate_data(line):
                print(f"Reading from {port.device}")
                read_from_port(ser)
            else:
                print(f"Data from {port.device} does not match expected format")
            ser.close()
    except serial.SerialException:
        print(f"Could not open port {port.device}")