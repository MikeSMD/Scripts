import bpy

obj = bpy.context.active_object
mesh = obj.data

mesh.calc_loop_triangles()

print("float vertices[] = {")

for tri in mesh.loop_triangles:
    for loop_index in tri.loops:
        v = mesh.vertices[mesh.loops[loop_index].vertex_index].co
        print(f"    {v.x:.6f}f, {v.y:.6f}f, {v.z:.6f}f,")
        
print("};")

