
data = open("day06.txt", "r").read()

def readStartOfMessageMarker(length):

	index = 0
	code = []
	for char in data:

		index+=1

		code.append(char)
		if len(set(code)) == length:
			return index
		if len(code) == length:
			del code[0]


print("Part 1: ", readStartOfMessageMarker(4))
print("Part 2: ", readStartOfMessageMarker(14))

## golf version for fun

#d = open("t", "r").read()
#def r(l):
#	i = 0
#	c = []
#	for x in d:
#		i+=1
#		c.append(x)
#		if len(set(c)) == l:
#			return i
#		if len(c) == l:
#			del c[0]
#print(r(4),r(14))

