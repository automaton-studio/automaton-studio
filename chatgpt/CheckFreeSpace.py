import shutil
import sys

# Method 1
# # Function to check free disk space
# def check_free_space(drive_letter):
#     total, used, free = shutil.disk_usage(drive_letter)
#     return free

# # Main function
# if __name__ == '__main__':
#     if len(sys.argv) != 2:
#         print("Usage: python check_disk_space.py <drive_letter>")
#         sys.exit(1)

#     drive_letter = sys.argv[1]
#     free_space = check_free_space(drive_letter)
#     print(f"Free space on {drive_letter}: {free_space} bytes")

# Method 2
drive_letter = sys.argv[1]
total, used, free = shutil.disk_usage(drive_letter)
print(f"Free space on {drive_letter}: {free} bytes")