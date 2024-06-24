'''
CODE=[mode,force,options,light_breathe_interval,R,G,B]

Modes: LOCK_STATE,SWITCH_RATCHET,FINE_DAMP;
Recommended Force: 5(LOCK_STATE),0.5 (SWITCH_RATCHET);
Recommended Light_breathe_interval: 5;

'''



import serial
import time
import serial.tools.list_ports

# Get a list of available serial ports
print("Available serial ports:")
print("=======================================")
for port in serial.tools.list_ports.comports():
    print(port.device)
print("=======================================")
print("")

blacklist = ["/dev/cu.wlan-debug", "/dev/cu.MovsMacBookAir", "/dev/cu.Bluetooth-Incoming-Port"]
ports = [port.device for port in serial.tools.list_ports.comports() if port.device not in blacklist]
ports.sort(reverse=True)


# Define the serial ports and their baud rates
port1 = ports[0] if len(ports) > 0 else None
port2 = ports[1] if len(ports) > 1 else None

print(f"Port 1: {port1}")
print(f"Port 2: {port2}")
baud_rate1 = 115200
baud_rate2 = 9600

# Open the serial ports
serial_port1 = serial.Serial(port1, baud_rate1, timeout=1)
serial_port2 = serial.Serial(port2, baud_rate2, timeout=1)


def send_to_port(serial_port, message):
    """Send a message to the specified serial port.
    
    Args:
        serial_port (serial.Serial): The serial port object to send the message to.
        message (str): The message to send.
    """
    if message.split(',')[0]=="SWITCH_RATCHET":
        message1="LOCK_STATE,5,0"
        serial_port.write(message1.encode('utf-8'))
        time.sleep(1)
    serial_port.write(message.encode('utf-8'))

while(True):
    send_to_port(serial_port2,input("KNOB CONTROL CODE:"))

