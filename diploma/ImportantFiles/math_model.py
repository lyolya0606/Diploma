from scipy.integrate import solve_ivp
import matplotlib.pyplot as plt
import json
import math


# ks1 = 7.016
# ks3 = 4.624
# ks_4 = 349.97
# ks5 = 217.516
# ks7 = 0.399
# k_7 = 1.166e-4
# ks12 = 0.014
# ks_13 = 11.073
# ks14 = 4.67
# ks16 = 0.271
# k_16 = 0.199
# ks17 = 4.162e-4
# k_17 = 13.465
# ks18 = 9.347e-6
# ks20 = 1.23e6
# k_20 = 2.104
# ks21 = 4.638e-4
# k_21 = 1.345
# kA = 3.543e-5
# kB = 6.177e12
# kC = 24.09
#
#
#
# k12 = 0
# kD = 0
# kE = 0

k1 = 0
k2 = 0
k3 = 0
k4 = 0
k5 = 0
k6 = 0
k7 = 0
k8 = 0
k9 = 0
k10 = 0
k11 = 0
k12 = 0
k13 = 0
k14 = 0
k15 = 0
k16 = 0
k17 = 0
k18 = 0
k19 = 0
k20 = 0
k21 = 0


def deriv(t, y):
    c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12, c13, c14, c15, c16, c17, c18, c19, c20, c21, c22, c23 = y

    w1 = k1 * c1 * c2 * c3
    w2 = k2 * c4
    w3 = k3 * c3 * c5
    w4 = k4 * c8
    w5 = k5 * c2 * c3 * c6
    w6 = k6 * c9
    w7 = k7 * c3 * c5
    w8 = k8 * c2 * c11
    w9 = k9 * c5 * c11
    w10 = k10 * c3 * c12
    w11 = k11 * c13
    w12 = k12 * c3 * c10
    w13 = k13 * c14
    w14 = k14 * c2 * c3 * c15
    w15 = k15 * c16
    w16 = k16 * c3 * c10
    w17 = k17 * c3 * c17
    w18 = k18 * c3 * c17 * c17
    w19 = k19 * c19
    w20 = k20 * c3 * c21
    w21 = k21 * c3 * c22

    c_1 = -w1 + w11
    c_2 = -w1 - w5 - w8 - w14
    c_3 = -w1 + w2 - w3 + w4 - w5 + w6 - w7 + w8 + w9 - w10 + w11 - w12 + w13 - w14 + w15 - w16 - w17 - w18 + w19 - w20 - w21
    c_4 = w1 - w2
    c_5 = w2 - w3 - w7 - w9
    c_6 = w4 - w5
    c_7 = w4 + w8 + w11 + w13
    c_8 = w3 - w4
    c_9 = w5 - w6
    c_10 = w6 + w7 - w12 - w16
    c_11 = w7 - w8 - w9 + w16 + w17 + w20 + w21
    c_12 = w9 - w10
    c_13 = w10 - w11
    c_14 = w12 - w13
    c_15 = w13 - w14
    c_16 = w14 - w15
    c_17 = w15 + w16 - w17 - 2 * w18
    c_18 = w17
    c_19 = w18 - w19
    c_20 = w19
    c_21 = w19 - w20
    c_22 = w20 - w21
    c_23 = w21
    return c_1, c_2, c_3, c_4, c_5, c_6, c_7, c_8, c_9, c_10, c_11, c_12, c_13, c_14, c_15, c_16, c_17, c_18, c_19, c_20, c_21, c_22, c_23


def arrenius(A, E, t):
    k_calc = []
    for i in range(len(A)):
        k_calc.append(math.exp(A[i]) * math.exp(-E[i] * 1000 / (8.31 * (t + 273))))
    return k_calc


def calculate_math_model(concentration, multiplier, energy, temperature, contact_time, method_name):
    global k1, k2, k3, k4, k5, k6, k7, k8, k9, k10, k11, k12, k13, k14, k15, k16, k17, k18, k19, k20, k21
    
    reaction_speed = arrenius(multiplier, energy, temperature)
    k1, k2, k3, k4, k5, k6, k7, k8, k9, k10, k11, k12, k13, k14, k15, k16, k17, k18, k19, k20, k21 = reaction_speed
    t0, tf = 0, contact_time
    y0 = concentration
    soln = solve_ivp(deriv, (t0, tf), y0, method=method_name)
    sol_dict = {'t': list(soln.t)}
    for i in range(soln.y.shape[0]):
        sol_dict.update({f'y_{i}': list(soln.y[i])})

    return json.dumps(sol_dict)


