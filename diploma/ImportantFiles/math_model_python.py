from scipy.integrate import solve_ivp
import matplotlib.pyplot as plt
import json
import math


def arrenius(A, E, t):
    k_calc = []
    for i in range(len(A)):
        k_calc.append(math.exp(A[i]) * math.exp(-E[i] * 1000 / (8.31 * (t + 273))))
    return k_calc


# k = arrenius(a, e, 400)
# w = k.copy()
# c_j = [0] * len(c_start)

matrix = []

def deriv(t, y):
    c = y
    w = k.copy()
    for i in range(len(matrix)):
        for j in range(len(matrix[i])):
            if matrix[i][j] < 0:
                w[i] *= c[j] ** abs(matrix[i][j])
    c_j = [0] * len(c)

    for i in range(len(matrix)):
        for j in range(len(matrix[i])):
            c_j[j] += matrix[i][j] * w[i]

    return c_j


def calculate_math_model(concentration, multiplier, energy, temperature, contact_time, method_name, got_matrix):
    global k, matrix
    k = arrenius(multiplier, energy, temperature)
    matrix = got_matrix
    # w = k.copy()
    # c_j = [0] * len(c_start)

    reaction_speed = arrenius(multiplier, energy, temperature)
    k = reaction_speed
    t0, tf = 0, contact_time
    y0 = concentration
    soln = solve_ivp(deriv, (t0, tf), y0, method=method_name)
    sol_dict = {'t': list(soln.t)}
    for i in range(soln.y.shape[0]):
        sol_dict.update({f'y_{i}': list(soln.y[i])})

    return json.dumps(sol_dict)

