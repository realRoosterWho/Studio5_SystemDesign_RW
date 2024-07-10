import serial

class SenderTest:
    def __init__(self):
        self.serial_port = None

    def send_message(self):
        # Let the user manually select the port
        port = input("Please enter the port you want to use: ")

        # Let the user manually select the baud rate
        baud_rate = int(input("Please enter the baud rate you want to use: "))

        # Open the serial port
        self.serial_port = serial.Serial(port, baud_rate, timeout=1)

        # Let the user manually input the message
        message = input("Please enter the message you want to send: ")

        # Add a newline character to the end of the message
        message += '\n'

        # Send the message
        self.serial_port.write(message.encode('utf-8'))
        print(f"Sent message to {self.serial_port.port} at baud rate {self.serial_port.baudrate}: {message}")

        # Close the serial port
        self.serial_port.close()

sender_test = SenderTest()
sender_test.send_message()