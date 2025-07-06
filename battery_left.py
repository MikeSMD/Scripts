import subprocess
import time

def charging():
    output = subprocess.check_output(['cat', '/sys/class/power_supply/BAT1/status']).decode().strip()
    return ( output == "Charging" or output == "Full" )


def get_per():
    output = subprocess.check_output(['cat', '/sys/class/power_supply/BAT1/capacity']).decode().strip()
    return output 


def cpu_read():
    output = subprocess.check_output(['cat', '/proc/stat']).decode()
    first_line = output.splitlines()[0]
    parts = first_line.split()
    values = list(map(int, parts[1:]))
    return values

# procentualni cas idle v 1 sekunde cpu
def cpu_load(interval=1):
    t1 = cpu_read()
    time.sleep(interval)
    t2 = cpu_read()

    idle1 = t1[3] + t1[4]  # idle + iowait
    idle2 = t2[3] + t2[4]

    total1 = sum(t1)
    total2 = sum(t2)

    totald = total2 - total1
    idled = idle2 - idle1

    if totald == 0:
        return 0.0

    cpu_usage = (totald - idled) * 100.0 / totald
    return cpu_usage
def get_jas():
    try:
        result = subprocess.check_output(['brightnessctl', 'g']).decode()
        return result 
    except Exception as e:
        print(f"exc {e}")
        return None
def get_temp():
    try:    
        result = subprocess.check_output(['cat', '/sys/class/thermal/thermal_zone0/temp']).decode();
        return result;
    except Exception as e:
        print(f"{e}")
        return None 
# -----------------------------------------

def fuzzy_linear_decreasing(v: int, b: int, c: int) -> float:
    if v >= c or v <= b :
        return 0.0
    else:
        return (c - v) / ( c - b )

def fuzzy_linear_increasing(v: int, b: int, c: int) -> float:
    if ( v <= b or v >= c ):
        return 0.0
    else:
        return 1-(c - v) / ( c - b )


def fuzzy_triangle(v:int, b:int,peak:int, c:int )->float:
    if( v >= c or v <= b ):
        return 0.0
    elif v < peak:
        return ( v - b ) / ( peak - b )
    else:
        return ( c - v ) / ( c - peak )
def fuzzify_jas(x):
    return {
        "low": fuzzy_linear_decreasing(x, 0, 80),
        "medium": fuzzy_triangle(x,60, 100, 140),
        "high": fuzzy_triangle(x, 100, 140, 180),
        "max": fuzzy_linear_increasing(x, 130, 200),
    }
def fuzzify_temp(x):
    return {
        "low": fuzzy_linear_decreasing(x, 30, 50),
        "medium": fuzzy_triangle(x,45, 57, 70),
        "high": fuzzy_triangle(x, 65, 75, 85),
        "max": fuzzy_linear_increasing(x, 80, 95),
    }

def fuzzify_usage(x):
    return {
        "low": fuzzy_linear_decreasing(x, 0, 35),
        "medium": fuzzy_triangle(x,25, 45, 60),
        "high": fuzzy_triangle(x, 45, 67, 90),
        "max": fuzzy_linear_increasing(x, 80, 100),
    }
def fuzzify_per(x):
    return {
        "low": fuzzy_linear_decreasing(x, 0, 35),
        "medium": fuzzy_triangle(x,30, 45, 60),
        "high": fuzzy_triangle(x, 55, 70, 90),
        "max": fuzzy_linear_increasing(x, 80, 100),
    }

def main():
    brightness = fuzzify_jas( int ( get_jas() ) )
    temp = fuzzify_temp ( int ( get_temp() ) / 1000 )
    usage = fuzzify_usage ( int ( cpu_load() ) )
    #charge = fuzzify_charging( charging() )
    per =fuzzify_per( int (get_per() ) )
    
    



    print(f"Jas obrazovky: {brightness}")
    print(f"teplota jednotky: {temp}")
    print(f"usage {usage}")
    print(f"per {per}")



if __name__ == "__main__":
    main()
