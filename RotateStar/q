mport bpy
import bmesh

# Funkce pro výpis souřadnic vrcholů v lokálních souřadnicích
def print_vertices(obj):
    # Ujistíme se, že je objekt triangulovaný
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    
    # Přejdeme do Edit Mode a triangulujeme
    bpy.ops.object.mode_set(mode='EDIT')
    bm = bmesh.from_edit_mesh(obj.data)
    bmesh.ops.triangulate(bm, faces=bm.faces[:])
    bmesh.update_edit_mesh(obj.data)
    bpy.ops.object.mode_set(mode='OBJECT')
    
    # Projdeme všechny polygony (nyní trojúhelníky)
    for poly in obj.data.polygons:
        # Pro každý trojúhelník vypíšeme jeho vrcholy v lokálních souřadnicích
        for vert_idx in poly.vertices:
            vert = obj.data.vertices[vert_idx].co  # Lokální souřadnice
            # Formát x,y,z
            print(f"{vert.x},{vert.y},{vert.z}")

# Vyber aktivní objekt
obj = bpy.context.active_object
if not obj or obj.type != 'MESH':
    raise ValueError("Vyber prosím mesh objekt!")

# Spusť výpis
print_vertices(obj)
