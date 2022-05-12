from math import pow
from math import pi as PI

# credit : Berke Algül

class Solver(object):
    def __init__(self):

        self.diameters = [4, 6, 8, 10, 12, 16, 20, 24, 32, 40, 48, 64, 80, 96, 128]
        self.g = 9.81
        self.v = 1.3 * pow(10,-6)

        self.x = {'A': 2000, 'B': 4000, 'C': 3900, 'D': 4400, 'E': 4600}
        self.y = {'A': 1500, 'B': 2500, 'C': 500, 'D': 800, 'E': 2200}
        self.h = {'A': 121, 'B': 24, 'C': 11, 'D': 53, 'E': 74}
        self.Q = {'B': 0.016, 'C': 0.017, 'D': 0.022, 'E': 0.035}

        self.fB = [0.02388, 0.02388, 0.02427, 0.02446, 0.02464, 0.02501,0.02535,0.02569, 0.02633,0.02694, 0.02752, 24485.37586, 0.02958, 0.03218, 0.02492]
        self.fC = [0.02385, 0.02404, 0.02422, 0.02440,0.02458,0.02525,0.02557,0.02619,0.02677,0.02732,0.02835, 0.029, 0.02930,0.03018, 0.03180]
        self.fD = [0.02377, 0.02391,0.02406, 0.02420 , 0.02434,0.02461,0.02488,0.02513, 0.02563, 0.02611, 0.02656, 0.02741, 0.02821, 0.02896, 0.03034]
        self.fE = [0.02366, 0.02375, 0.02384, 0.02393, 0.02402, 0.02420, 0.02438,0.02455, 0.02488,0.02521, 0.02552, 0.02612, 0.02669, 0.02722, 0.02823]

        self.ReB = []
        self.ReC = []
        self.ReD = []
        self.ReE = []

        self.kB = []
        self.kC = []
        self.kD = []
        self.kE = []

        self.reynolds()
        self.calculate_distances()
        self.first_topology()
        self.second_topology()
        self.third_topology()

    def reynolds(self):
        for D in self.diameters:
            ReB = ((4*self.Q['B'])/(PI*(D*0.01)))/self.v
            ReC = ((4*self.Q['C'])/(PI*(D*0.01)))/self.v
            ReD = ((4*self.Q['D'])/(PI*(D*0.01)))/self.v
            ReE = ((4*self.Q['E'])/(PI*(D*0.01)))/self.v

            self.ReB.append(float("{0:.5f}".format(ReB)))
            self.ReC.append(float("{0:.5f}".format(ReC)))
            self.ReD.append(float("{0:.5f}".format(ReD)))
            self.ReE.append(float("{0:.5f}".format(ReE)))
        
        print("\nReynold numbers\n")
        print("Re B: ",self.ReB)
        print("Re C: ",self.ReC)
        print("Re D: ",self.ReD)
        print("Re E: ",self.ReE)

    def calculate_distances(self):
        L_AB = (((self.x['A'] - self.x['B'])**2) + ((self.y['A'] - self.y['B'])**2)) ** 0.5
        L_AC = (((self.x['A'] - self.x['C'])**2) + ((self.y['A'] - self.y['C'])**2)) ** 0.5
        L_AD = (((self.x['A'] - self.x['D'])**2) + ((self.y['A'] - self.y['D'])**2)) ** 0.5
        L_AE = (((self.x['A'] - self.x['E'])**2) + ((self.y['A'] - self.y['E'])**2)) ** 0.5
        L_ED = (((self.x['E'] - self.x['D'])**2) + ((self.y['E'] - self.y['D'])**2)) ** 0.5
        L_EC = (((self.x['E'] - self.x['C'])**2) + ((self.y['E'] - self.y['C'])**2)) ** 0.5

        print("\nDistances\n")
        print("Lenght A-B: ",L_AB)
        print("Lenght A-E: ",L_AE)
        print("Lenght A-D: ",L_AD)
        print("Lenght A-C: ",L_AC)
        print("Lenght E-D: ",L_ED)
        print("Lenght E-C: ",L_EC)
        print("")

    def first_topology(self):
        self.kB.clear()
        self.kC.clear()
        self.kD.clear()
        self.kE.clear()

        for i in range(len(self.diameters)):
            
            kB = ((94*2*self.g)/((4*self.Q['B']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fB[i] * 2330.0679/(self.diameters[i] * 0.01)))
            kE = ((60*2*self.g)/((4*self.Q['E']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fE[i] * 2752.5824/(self.diameters[i] * 0.01)))
            kD = ((71*2*self.g)/((4*self.Q['D']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fD[i] * 2571/(self.diameters[i] * 0.01)))
            kC = ((85*2*self.g)/((4*self.Q['C']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fC[i] * 2232.0910/(self.diameters[i] * 0.01)))
            self.kB.append(kB)
            self.kE.append(kE)
            self.kD.append(kD)
            self.kC.append(kC)
        
        print("\n1. Topology\n")
        print("Kb: ", self.kB)
        print("Kc: ", self.kC)
        print("Kd: ", self.kD)
        print("Ke: ", self.kE)
        print("")

    def second_topology(self):
        self.kB.clear()
        self.kC.clear()
        self.kD.clear()
        self.kE.clear()

        for i in range(len(self.diameters)):
            kB = ((94*2*self.g)/((4*self.Q['B']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fB[i] * 2330.0679/(self.diameters[i] * 0.01)))
            kE = ((60*2*self.g)/((4*self.Q['E']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fE[i] * 2752.5824/(self.diameters[i] * 0.01)))
            kD = ((11*2*self.g)/((4*self.Q['D']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fD[i] * 1425.221/(self.diameters[i] * 0.01)))
            kC = ((25*2*self.g)/((4*self.Q['C']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fC[i] * 1863.47763/(self.diameters[i] * 0.01)))
            self.kB.append(kB)
            self.kE.append(kE)
            self.kD.append(kD)
            self.kC.append(kC)
        
        print("\n2.Topology\n")

        print("Kb: ", self.kB)
        print("Kc: ", self.kC)
        print("Ke: ", self.kE)
        print("Kd: ", self.kD)
        print("")

    def third_topology(self):
        self.kB.clear()
        self.kC.clear()
        self.kD.clear()
        self.kE.clear()

        for i in range(len(self.diameters)):   
            kB = ((94*2*self.g)/((4*self.Q['B']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fB[i] * 2330.0679/(self.diameters[i] * 0.01)))
            kE = ((60*2*self.g)/((4*self.Q['E']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fE[i] * 2752.5824/(self.diameters[i] * 0.01)))
            kD = ((11*2*self.g)/((4*self.Q['D']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fD[i] * 1425.221/(self.diameters[i] * 0.01)))
            kC = ((85*2*self.g)/((4*self.Q['C']/((PI * ((self.diameters[i] * 0.01)**2))))**2) - (self.fC[i] * 2232.0910/(self.diameters[i] * 0.01)))
            self.kB.append(kB)
            self.kE.append(kE)
            self.kD.append(kD)
            self.kC.append(kC)

        print("\n3. Topology\n")

        print("Kb: ", self.kB)
        print("Kc: ", self.kC)
        print("Ke: ", self.kE)
        print("Kd: ", self.kD)
        print("")

if __name__ == "__main__":
    Solver()