import tkinter as tk
import numpy as np

class Canvas28x28:
    def __init__(self):
        self.root = tk.Tk()
        self.root.title("MNIST Canvas 28x28")
        self.canvas_size = 280  # 28*10 pixely pro kreslení
        self.pixel_size = 10
        self.canvas = tk.Canvas(self.root, width=self.canvas_size, height=self.canvas_size, bg='white')
        self.canvas.pack()
        self.pixels = np.zeros((28,28), dtype=np.float32)

        self.canvas.bind("<B1-Motion>", self.paint)
        button = tk.Button(self.root, text="Done", command=self.done)
        button.pack()
        self.done_flag = False

    def paint(self, event):
        x = event.x // self.pixel_size
        y = event.y // self.pixel_size
        if 0 <= x < 28 and 0 <= y < 28:
            self.pixels[y,x] = 1.0
            self.canvas.create_rectangle(x*self.pixel_size, y*self.pixel_size,
                                         (x+1)*self.pixel_size, (y+1)*self.pixel_size,
                                         fill='black')

    def done(self):
        self.done_flag = True
        self.root.destroy()

    def run(self):
        self.root.mainloop()
        return self.pixels.flatten()  # vrací 784 prvků ve formátu MNIST

if __name__ == "__main__":
    canvas = Canvas28x28()
    data = canvas.run()
    # vypíše jako řádek s mezerami
    print(" ".join(map(str, data)))

