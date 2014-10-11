import itertools
import copy
import Queue


class D(object):
    def __init__(self):
        self.start = 0
        self.end = 0
        self.length = 0

    def __repr__(self):
        return '<{} ({}:{})>'.format(self.length, self.start, self.end)


def d_len(string):
    length = 0

    Ds = []
    d = None
    for i in xrange(len(string)):
        if string[i] != 'D':
            if length > 0:
                d.end = i
                d.length = length
                Ds.append(d)
            length = 0

        else:
            if length == 0:
                d = D()
                d.start = i
            length += 1

    if length > 0:
        d.end = len(string)
        d.length = length
        Ds.append(d)

    return Ds


def solve(k, string):
    Ds = d_len(string)
    print Ds


    # for d in Ds:
    #     print d





def longest(fragments):
    SD = 0
    DD = 0
    DS = 0

    for fragment in fragments:
        if fragment.count('S') == 0:
            DD += len(fragment)

        elif fragment.endswith('D'):
            length = 0
            for f in fragment[::-1]:
                if f != 'D':
                    break
                length += 1

            SD = max(SD, length)

        elif fragment.startswith('D'):
            length = 0
            for f in fragment:
                if f != 'D':
                    break
                length += 1

            DS = max(DS, length)

    return SD + DD + DS


def compress(string):
    flag = False
    c = ""
    for s in string:
        if s == "S":
            if not flag:
                flag = True
                c += s
        else:
            flag = False
            c += s
    return c

def brute(k, string):
    Ds = d_len(string)

    bo = []
    for d in Ds:
        bo.append(d.start)
        bo.append(d.end)

    if 0 in bo:
        bo.remove(0)
    if len(string) in bo:
        bo.remove(len(string))

    maxi = 0
    k = min(k, len(bo))
    for bos in itertools.combinations(bo, k):
        bos = list(bos)
        s = copy.deepcopy(string)
        arrays = []
        bos.append(0)
        bos.sort()

        for i in xrange(1, len(bos)):
            arrays.append(s[bos[i - 1]:bos[i]])
        arrays.append(s[bos[-1]:])
        maxi = max(maxi, longest(arrays))

    return maxi

if __name__ == "__main__":
    with open("fil02.in", "r") as f:
        N = int(f.readline())
        for i in xrange(N):
            k, string = f.readline().split()

            l = brute(int(k), compress(string))
            print l



