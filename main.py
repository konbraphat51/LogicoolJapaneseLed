from logiled import LogitechLed, load_dll

load_dll()

led = LogitechLed()

led.set_lighting(100, 0, 0)
input("Press enter to shutdown SDK...")
led.shutdown()