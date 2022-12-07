
data = open("day06.txt", "r").read()

def readStartOfMessageMarker(length):

	index = 0
	code = []
	for char in data:

		index+=1

		code.append(char)
		if len(set(code)) == length:
			break
		if len(code) == length:
			del code[0]

	return index

print("Part 1: ", readStartOfMessageMarker(4))
print("Part 1: ", readStartOfMessageMarker(14))

