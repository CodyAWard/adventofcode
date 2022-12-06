
data = open("day04.txt", "r").read()
data = data.splitlines()

total = 0

for line in data:
	split = line.split(",")
	a = split[0].split("-")
	b = split[1].split("-")

	if int(a[0]) <= int(b[0]) and int(a[1]) >= int(b[1]) \
	or int(b[0]) <= int(a[0]) and int(b[1]) >= int(a[1]):
		total+=1

print("Part 1: ", total)


total = 0

for line in data:
	split = line.split(",")
	a = split[0].split("-")
	b = split[1].split("-")

	if int(a[0]) <= int(b[1]) and int(a[1]) >= int(b[0]) \
	or int(b[0]) <= int(a[1]) and int(b[1]) >= int(a[0]):
		total+=1

print("Part 2: ", total)