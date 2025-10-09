import tkinter as tk
import numpy as np
from PIL import Image
import os
import tkinter.messagebox as messagebox

class MNISTCanvas:
    def __init__(self):
        self.root = tk.Tk()
        self.root.title("MNIST Digit Drawing Canvas")
        self.canvas_size = 280  # 28*10 for display
        self.brush_width = 20   # Brush size for smooth drawing
        self.pixel_size = self.canvas_size // 28
        
        # Main frame
        self.frame = tk.Frame(self.root, padx=10, pady=10)
        self.frame.pack()
        
        # Canvas
        self.canvas = tk.Canvas(self.frame, width=self.canvas_size, height=self.canvas_size, bg='white')
        self.canvas.pack()
        
        # Store pixels for final processing (white background: 255)
        self.pixels = np.full((self.canvas_size, self.canvas_size), 255, dtype=np.uint8)
        
        # Bind mouse events
        self.last_x, self.last_y = None, None
        self.canvas.bind('<Button-1>', self.activate_paint)
        self.canvas.bind('<B1-Motion>', self.paint)
        self.canvas.bind('<ButtonRelease-1>', self.reset)
        
        # Buttons
        self.button_frame = tk.Frame(self.frame)
        self.button_frame.pack(fill=tk.X, pady=5)
        
        tk.Button(self.button_frame, text="Clear", command=self.clear).pack(side=tk.LEFT, padx=5)
        tk.Button(self.button_frame, text="Save", command=self.save_image).pack(side=tk.LEFT, padx=5)
        tk.Button(self.button_frame, text="Done", command=self.done).pack(side=tk.LEFT, padx=5)
        
        self.done_flag = False
    
    def activate_paint(self, event):
        self.paint(event)
    
    def paint(self, event):
        x, y = event.x, event.y
        if 0 <= x < self.canvas_size and 0 <= y < self.canvas_size:
            # Draw on canvas
            if self.last_x and self.last_y:
                # Draw line for smooth strokes
                self.canvas.create_line(self.last_x, self.last_y, x, y, 
                                      fill='black', width=self.brush_width, capstyle=tk.ROUND)
            else:
                # Draw initial dot
                self.canvas.create_oval(x - self.brush_width//2, y - self.brush_width//2,
                                       x + self.brush_width//2, y + self.brush_width//2,
                                       fill='black', outline='black')
            
            # Update pixel array (approximate brush effect)
            r = self.brush_width // 2
            for dy in range(-r, r + 1):
                for dx in range(-r, r + 1):
                    px, py = x + dx, y + dy
                    if 0 <= px < self.canvas_size and 0 <= py < self.canvas_size:
                        # Distance-based intensity (center darker)
                        dist = np.sqrt(dx**2 + dy**2)
                        if dist <= r:
                            intensity = max(0, self.pixels[py, px] - int(255 * (1 - dist/r)))
                            self.pixels[py, px] = intensity
            
            self.last_x, self.last_y = x, y
    
    def reset(self, event):
        self.last_x, self.last_y = None, None
    
    def clear(self):
        self.canvas.delete("all")
        self.pixels = np.full((self.canvas_size, self.canvas_size), 255, dtype=np.uint8)
    
    def save_image(self):
        try:
            i = 1
            while os.path.exists(f"mnist_digit_{i}.png"):
                i += 1
            img = Image.fromarray(self.pixels, mode='L')
            img.save(f"mnist_digit_{i}.png")
            messagebox.showinfo("Success", f"Image saved as mnist_digit_{i}.png")
        except Exception as e:
            messagebox.showerror("Error", f"Failed to save: {str(e)}")
    
    def done(self):
        self.done_flag = True
        self.root.destroy()
    
    def run(self):
        self.root.mainloop()
        if not self.done_flag:
            return None
        
        # Convert pixels to PIL Image and resize to 28x28
        img = Image.fromarray(self.pixels, mode='L')
        small_img = img.resize((28, 28), Image.Resampling.LANCZOS)
        
        # Convert to numpy, normalize, and invert for MNIST
        pixels = np.array(small_img, dtype=np.float32) / 255.0  # 0 black, 1 white
        pixels = 1.0 - pixels  # 1 digit, 0 background, with grays
        
        return pixels.flatten()

if __name__ == "__main__":
    try:
        canvas = MNISTCanvas()
        data = canvas.run()
        if data is not None:
            print(" ".join(map(lambda x: f"{x:.3f}", data)))
    except Exception as e:
       print(f"Error: {str(e)}")
