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
    if v > c or v < b :
        return 0.0
    else:
        return (c - v) / ( c - b )

def fuzzy_linear_increasing(v: int, b: int, c: int) -> float:
    if ( v < b or v > c ):
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
    print ( x )
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
rules = [
    {"inputs": {"jas": "max", "temp": "max", "usage": "max", "per": "low"}, "output": {"vydrz": "low"}, "operator": "AND"},
    {"inputs": {"jas": "high", "temp": "high", "usage": "high", "per": "low"}, "output": {"vydrz": "low"}, "operator": "AND"},
    {"inputs": {"jas": "high", "temp": "high", "usage": "high", "per": "medium"}, "output": {"vydrz": "low"}, "operator": "AND"},
    {"inputs": {"jas": "medium", "temp": "medium", "usage": "medium", "per": "low"}, "output": {"vydrz": "low"}, "operator": "AND"},
    {"inputs": {"jas": "medium", "temp": "medium", "usage": "medium", "per": "medium"}, "output": {"vydrz": "medium"}, "operator": "AND"},
    {"inputs": {"jas": "low", "temp": "low", "usage": "low", "per": "medium"}, "output": {"vydrz": "high"}, "operator": "AND"},
    {"inputs": {"jas": "low", "temp": "low", "usage": "low", "per": "high"}, "output": {"vydrz": "max"}, "operator": "AND"},
    {"inputs": {"jas": "high", "temp": "medium", "usage": "low", "per": "medium"}, "output": {"vydrz": "medium"}, "operator": "AND"},
    {"inputs": {"jas": "low", "temp": "medium", "usage": "high", "per": "medium"}, "output": {"vydrz": "medium"}, "operator": "AND"},
    {"inputs": {"jas": "medium", "temp": "low", "usage": "medium", "per": "high"}, "output": {"vydrz": "high"}, "operator": "AND"},
    {"inputs": {"jas": "high", "temp": "low", "usage": "low", "per": "max"}, "output": {"vydrz": "high"}, "operator": "AND"},
    {"inputs": {"jas": "high", "temp": "medium", "usage": "medium", "per": "max"}, "output": {"vydrz": "medium"}, "operator": "AND"},
    {"inputs": {"jas": "high", "temp": "medium", "usage": "medium", "per": "medium"}, "output": {"vydrz": "low"}, "operator": "AND"},
    {"inputs": {"jas": "low", "temp": "medium", "usage": "low", "per": "medium"}, "output": {"vydrz": "medium"}, "operator": "AND"},
{"inputs": {"jas": "medium", "temp": "high", "usage": "low", "per": "medium"}, "output": {"vydrz": "medium"}, "operator": "AND"},
    {"inputs": {"jas": "low", "temp": "high", "usage": "medium", "per": "low"}, "output": {"vydrz": "medium"}, "operator": "AND"},
    {"inputs": {"jas": "low", "temp": "high", "usage": "high", "per": "low"}, "output": {"vydrz": "low"}, "operator": "AND"},
    {"inputs": {"jas": "max", "temp": "medium", "usage": "medium", "per": "medium"}, "output": {"vydrz": "low"}, "operator": "AND"},
    {"inputs": {"jas": "medium", "temp": "low", "usage": "high", "per": "medium"}, "output": {"vydrz": "medium"}, "operator": "AND"},
    {"inputs": {"jas": "low", "temp": "medium", "usage": "high", "per": "low"}, "output": {"vydrz": "medium"}, "operator": "AND"},
    {"inputs": {"jas": "high", "temp": "max", "usage": "medium", "per": "medium"}, "output": {"vydrz": "low"}, "operator": "AND"},
    {"inputs": {"jas": "max", "temp": "low", "usage": "low", "per": "high"}, "output": {"vydrz": "high"}, "operator": "AND"},
    {"inputs": {"jas": "max", "temp": "low", "usage": "low", "per": "high"}, "output": {"vydrz": "high"}, "operator": "AND"},
    {"inputs": {"jas": "max", "temp": "low", "usage": "low", "per": "low"}, "output": {"vydrz": "low"}, "operator": "AND"},
    {"inputs": {"jas": "low", "temp": "medium", "usage": "low", "per": "low"}, "output": {"vydrz": "medium"}, "operator": "AND"},]



def firing_strength(brightness, temp, usage, per, rule):
    firing_strength = min(
        brightness[rule["inputs"]["jas"]],
        temp[rule["inputs"]["temp"]],
        usage[rule["inputs"]["usage"]],
        per[rule["inputs"]["per"]]
    )
    return {rule["output"]["vydrz"]: firing_strength}


def evaluate_rules(brightness, temp, usage, per, rules):
    return [firing_strength(brightness, temp, usage, per, rule) for rule in rules]



def aggregate_outputs(rule_outputs):
    aggregated = {"low": 0.0, "medium": 0.0, "high": 0.0, "max": 0.0}
    for output in rule_outputs:
        for key, value in output.items():
            aggregated[key] = max(aggregated[key], value)
    return aggregated
boundries = {
    "low": 1,      # střed rozsahu (0+2)/2 = 1
    "medium": 3.5, # střed rozsahu (2+5)/2 = 3.5
    "high": 6.5,   # střed rozsahu (5+8)/2 = 6.5
    "max": 9       # střed rozsahu (8+10)/2 = 9
}

def defuzzification(boundries, aggregated):
    s = 0.0
    p = 0.0
    for k in ["low", "medium", "high", "max"]:
        s += boundries[k] * aggregated[k]  # Reprezentativní hodnota * příslušnost
        p += aggregated[k]
    return s / p if p != 0 else 0.0


def main():

    if ( charging() ):
        print("zarizeni se nabiji doba za jak dlouho se vybije je odhadnuta kdyby se nyni napajeni odpojilo")
    
    brightness = fuzzify_jas( int ( get_jas() ) )
    temp = fuzzify_temp ( int ( get_temp() ) / 1000 )
    usage = fuzzify_usage ( int ( cpu_load() ) )
    per =fuzzify_per( int (get_per() ) )
    
    rule_outputs = evaluate_rules(brightness, temp, usage, per, rules)
    aggregated = aggregate_outputs(rule_outputs)

    s = defuzzification(boundries,aggregated)



    print(f"Jas obrazovky: {brightness}")
    print(f"teplota jednotky: {temp}")
    print(f"usage {usage}")
    print(f"per {per}")
    print( f"aggregated {aggregated}" )   # max ze vsech pravidel dle vystpup
    print( f"odhadovana vydrz - {s}" )



if __name__ == "__main__":
    main()
