
import numpy as np
from torchvision import datasets

# stáhne trénovací a testovací data
mnist_train = datasets.MNIST(root='./data', train=True, download=True)
mnist_test = datasets.MNIST(root='./data', train=False, download=True)

# převod obrázky na numpy array a normalizace na [0,1]
X_train = mnist_train.data.numpy().reshape(-1, 28*28) / 255.0
y_train = mnist_train.targets.numpy()

X_test = mnist_test.data.numpy().reshape(-1, 28*28) / 255.0
y_test = mnist_test.targets.numpy()

# --- one-hot encoding labelů ---
num_classes = 10
y_train_onehot = np.eye(num_classes)[y_train]  # shape (60000, 10)
y_test_onehot = np.eye(num_classes)[y_test]    # shape (10000, 10)

# ulozeni do csv
np.savetxt('mnist_train_images.csv', X_train, delimiter=',')
np.savetxt('mnist_train_labels.csv', y_train_onehot, fmt='%d', delimiter=',')

np.savetxt('mnist_test_images.csv', X_test, delimiter=',')
np.savetxt('mnist_test_labels.csv', y_test_onehot, fmt='%d', delimiter=',')
#mameš 4 CSV soubory: trénovací obrázky, trénovací label, testovací obrázky, testovací label. 

