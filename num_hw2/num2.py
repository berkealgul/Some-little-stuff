# Berke Algül
# 030180166
# 25.05.2022

# polinomial parameters
b0 = 248.9273
b1 = -3.3995
b2 = 0.0240

# upper and lower bounds of integration
up = 120
low = 20

# step params
n_max = 20
n_min = 10
dm = 2
step_count = ((n_max-n_min)/2)+1

analytical_solution = 14856.23

def f(x):
    return b2*x*x + b1*x + b0

# n -> steps
def multiple_simpsons(n):
    result = 0
    b = up
    d = (up-low)/n
    for i in range(n):
        a = b-d
        result += simpsons(b, a)
        b = a
    return result

# b -> upper bound
# a -> lower bound
def simpsons(b, a):
    return ((b-a)*(f(a)+4*f((a+b)/2)+f(b)))/6

# n -> steps
def multiple_trapezoidal(n):
    result = 0
    b = up
    d = (up-low)/n
    for i in range(n):
        a = b-d
        result += trapezoidal(b, a)
        b = a
    return result

# b -> upper bound
# a -> lower bound
def trapezoidal(b, a):
    return ((b-a) *(f(b)+f(a)))/2

def main():
    print("2-a I Trapezoidal Rule")
    trap_result_mean = 0
    for i in range(n_min, n_max+dm, dm):
        result = multiple_trapezoidal(i)
        trap_result_mean += result
        print("n =", i, "|", result)
    trap_result_mean/=step_count

    print("---------------------")

    print("2-a II Simpsons 1/3 Rule")
    simp_result_mean = 0
    for i in range(n_min, n_max+dm, dm):
        result = multiple_simpsons(i)
        simp_result_mean += result
        print("n =", i, "|", result)
    simp_result_mean/=step_count
    print("---------------------")

    print("2-a III Errors")
    print("mean of trapezoidal rule = ", trap_result_mean)
    print("mean of simpsons 1/3 rule = ", simp_result_mean)
    trap_err = abs((analytical_solution-trap_result_mean)/trap_result_mean)*100
    simp_err = abs((analytical_solution-simp_result_mean)/simp_result_mean)*100
    print("error of trapezoidal rule = ", trap_err,"%")
    print("error of simpsons 1/3 rule = ", simp_err,"%")


if __name__ == "__main__":
    main()
