import serial
import serial.tools.list_ports
import json

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
            else:
                print("Invalid data received: ", line)
        except KeyboardInterrupt:
            break

ports = serial.tools.list_ports.comports()

# 黑名单串口
blacklist = ["/dev/cu.wlan-debug", "/dev/cu.MovsMacBookAir", "/dev/cu.Bluetooth-Incoming-Port"]

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