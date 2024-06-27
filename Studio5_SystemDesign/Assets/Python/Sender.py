'''
CODE=[mode,force,options,light_breathe_interval,R,G,B]

Modes: LOCK_STATE,SWITCH_RATCHET,FINE_DAMP;
Recommended Force: 5(LOCK_STATE),0.5 (SWITCH_RATCHET);
Recommended Light_breathe_interval: 5;

'''



import serial
import time
import serial.tools.list_ports
import json
import os
import hashlib

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
port1 = '/dev/cu.wchusbserial11320'  # Replace with your actual serial port
port2 = '/dev/cu.wchusbserial11330'  # Replace with your actual serial port
port3 = '/dev/cu.usbmodem113401'

print(f"Port 1: {port1}")
print(f"Port 2: {port2}")
baud_rate1 = 9600
baud_rate2 = 115200
baud_rate3 = 9600;

# Open the serial ports
serial_port1 = serial.Serial(port1, baud_rate1, timeout=1)
serial_port2 = serial.Serial(port2, baud_rate2, timeout=1)


def get_file_hash(file_path):
    """Get the SHA1 hash of a file."""
    with open(file_path, 'rb') as f:
        return hashlib.sha1(f.read()).hexdigest()


def read_json_file(file_path):
    """Read a JSON file and return its contents."""
    with open(file_path, 'r') as f:
        return json.load(f)


def send_to_port(serial_port, data, keys=None):
    """Send a message to the specified serial port.

    Args:
        serial_port (serial.Serial): The serial port object to send the message to.
        data (dict): The data to send.
        keys (list, optional): The keys of the data to send. If None, all data is sent.
    """
    if keys is None:
        message = ','.join(str(value) for value in data.values())
    else:
        message = ','.join(str(data[key]) for key in keys if key in data)
    serial_port.write(message.encode('utf-8'))
    print(f"Sent message: {message}")

def send_to_ports(data):
    """Send data to port2 and port3.

    Args:
        data (dict): The data to send.
    """
    # Open the serial port for port3
    serial_port3 = serial.Serial(port3, baud_rate3, timeout=1)

    # Send all data to port2
    send_to_port(serial_port2, data)

    # Send only the 'r', 'g', 'b' data to port3
    send_to_port(serial_port3, data, keys=['r', 'g', 'b'])

    # Close the serial port for port3
    serial_port3.close()


# The path to the JSON file
file_path = 'data_out.json'

# Get the initial file hash
last_file_hash = get_file_hash(file_path)

while True:
    # Wait for a while
    time.sleep(1)

    # Get the current file hash
    current_file_hash = get_file_hash(file_path)

    # If the file hash has changed, the file has been modified
    if current_file_hash != last_file_hash:
        # Update the last file hash
        last_file_hash = current_file_hash

        # Read the JSON file
        data = read_json_file(file_path)

        # Send the data
        send_to_ports(data)

# while(True):
#     send_to_port(serial_port2,input("KNOB CONTROL CODE:"))

