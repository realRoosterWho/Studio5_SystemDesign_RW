import serial
import json

# 读取串口/dev/cu.usbmodem13201
ser = serial.Serial('/dev/cu.usbmodem1201', 9600)

#循环地打印读取的串口，直到按下ESC
while True:
    try:
        # 读取串口数据
        line = ser.readline().decode().strip()
        # 将数据分割为三个部分
        parts = line.split(',')
        # Check if parts contains exactly three elements
        if len(parts) == 3:
            # 创建一个字典，将数据转换为JSON格式
            data = {
                'x': float(parts[0]),
                'y': float(parts[1]),
                'z': float(parts[2])
            }
            # 打印JSON数据
            print(json.dumps(data))
            # 打开一个文件用于写入数据
            with open('data.json', 'w') as f:
                # 将JSON数据写入文件
                json.dump(data, f)
        else:
            print("Invalid data received: ", line)
    except KeyboardInterrupt:
        break

ser.close()