class MyDeque:
    def __init__(self):
        self.__elements = []
        self.__head = 0
        self.__tail = 0

    def __str__(self):
        res = ''
        for i in range(self.__head, self.__tail):
            res += str(self.__elements[i]) + ' '
        return res

    def add(self, el):
        if self.__tail < len(self.__elements):
            self.__elements[self.__tail] = el
        else:
            self.__elements.append(el)

        self.__tail += 1

    def addleft(self, el):
        if self.__head > 0:
            self.__elements[self.__head] = el
            self.__head -= 1
        else:
            a = [el]
            for i in range(self.__head, self.__tail):
                a.append(self.__elements[i])
            self.__elements = a
            self.__tail += 1

    def pop(self):
        if self.__head >= self.__tail:
            raise IndexError('pop from empty deque')
        else:
            self.__tail -= 1
        return self.__elements[self.__tail]

    def popleft(self):
        if self.__head >= self.__tail:
            raise IndexError('pop from empty deque')
        else:
            self.__head += 1
            return self.__elements[self.__head - 1]
          
