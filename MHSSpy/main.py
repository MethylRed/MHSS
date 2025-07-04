# -*- coding: utf-8 -*-

import pulp
import csv
import utl
from collections import defaultdict

# 線形計画問題の定義
# 最大化問題を解く
problem = pulp.LpProblem('test', pulp.LpMaximize)

CsvSkill = "./data/MHWs_SKILL.csv"
CsvHead  = "./data/MHWs_HEAD.csv"
CsvBody  = "./data/MHWs_BODY.csv"
CsvArm   = "./data/MHWs_ARM.csv"
CsvWaist = "./data/MHWs_WST.csv"
CsvLeg   = "./data/MHWs_LEG.csv"
CsvDeco  = "./data/MHWs_DECO.csv"
CsvCharm = "./data/MHWs_CHARM.csv"
CsvSkillCondition = "./conditions/skillCondition.csv"


skills = utl.LoadSkills(CsvSkill)
head = utl.LoadEquips(utl.Kind.HEAD, CsvHead)
body = utl.LoadEquips(utl.Kind.BODY, CsvBody)
arm = utl.LoadEquips(utl.Kind.ARM, CsvArm)
wst = utl.LoadEquips(utl.Kind.WST, CsvWaist)
leg = utl.LoadEquips(utl.Kind.LEG, CsvLeg)
charm = utl.LoadCharm(utl.Kind.CHARM, CsvCharm)
deco = utl.LoadDeco(utl.Kind.DECO, CsvDeco)

skillCondition = utl.LoadSkillsCondition(CsvSkillCondition)

equips = head + body + arm + wst + leg + charm + deco

variables = {}
for equip in equips:
    if (equip.Kind == utl.Kind.DECO):
        variables[equip.Name] = pulp.LpVariable(equip.Name, 0, equip.HaveCount, cat=pulp.LpInteger)
    else:
        if equip.IsLock:
            variables[equip.Name] = pulp.LpVariable(equip.Name, 1, 1, cat=pulp.LpInteger)
        elif equip.IsExclude:
            variables[equip.Name] = pulp.LpVariable(equip.Name, 0, 0, cat=pulp.LpInteger)
        else:
            variables[equip.Name] = pulp.LpVariable(equip.Name, cat=pulp.LpBinary)

constraints = defaultdict(int)
for equip in equips:
    if equip.Kind == utl.Kind.WEAPON:
        constraints['WEAPON'] += variables[equip.Name]
    elif equip.Kind == utl.Kind.HEAD:
        constraints['HEAD'] += variables[equip.Name]
    elif equip.Kind == utl.Kind.BODY:
        constraints['BODY'] += variables[equip.Name]
    elif equip.Kind == utl.Kind.ARM:
        constraints['ARM'] += variables[equip.Name]
    elif equip.Kind == utl.Kind.WST:
        constraints['WST'] += variables[equip.Name]
    elif equip.Kind == utl.Kind.LEG:
        constraints['LEG'] += variables[equip.Name]
    elif equip.Kind == utl.Kind.CHARM:
        constraints['CHARM'] += variables[equip.Name]

    constraints['DEF'] += equip.Def * variables[equip.Name]
    constraints['FIRE'] += equip.ResFire * variables[equip.Name]
    constraints['WATER'] += equip.ResWater * variables[equip.Name]
    constraints['THUNDER'] += equip.ResThunder * variables[equip.Name]
    constraints['ICE'] += equip.ResIce * variables[equip.Name]
    constraints['DRAGON'] += equip.ResDragon * variables[equip.Name]

    for skill in equip.Skills:
        constraints[skill.Name] += skill.Level * variables[equip.Name]

    slotCount = utl.SlotCount(equip)
    if equip.SlotType == 0:
        for i in range(4):
            constraints['WEAPONSLOT'+str(i+1)] += slotCount[i] * variables[equip.Name]
    elif equip.SlotType == 1:
        for i in range(4):
            constraints['ARMORSLOT'+str(i+1)] += slotCount[i] * variables[equip.Name]

problem += constraints['DEF']

problem += 0 <= constraints['WEAPON'] <= 1
problem += 0 <= constraints['HEAD'] <= 1
problem += 0 <= constraints['BODY'] <= 1
problem += 0 <= constraints['ARM'] <= 1
problem += 0 <= constraints['WST'] <= 1
problem += 0 <= constraints['LEG'] <= 1
problem += 0 <= constraints['CHARM'] <= 1

for i in range(4):
    problem += 0 <= constraints['WEAPONSLOT'+str(i+1)]
for i in range(4):
    problem += 0 <= constraints['ARMORSLOT'+str(i+1)]

for skill in skillCondition:
    if skill.IsFixed:
        problem += skill.Level <= constraints[skill.Name] <= skill.Level
    else:
        problem += skill.Level <= constraints[skill.Name]

count = 0
maxcount = 1
while (1):
    status = problem.solve(pulp.PULP_CBC_CMD(msg = False))
    # print(status)
    if (pulp.LpStatus[status] == "Optimal") & (count < maxcount):
        for k, v in variables.items():
            if v.value() != 0:
                print(k)

        print(f"{int(constraints['DEF'].value())}, {int(constraints['FIRE'].value())}, {int(constraints['WATER'].value())}, "
              f"{int(constraints['THUNDER'].value())}, {int(constraints['ICE'].value())}, {int(constraints['DRAGON'].value())}")
        
        skillPrint = ""
        for skill in skillCondition:
            if type(constraints[skill.Name]) != int:
                if int(constraints[skill.Name].value()) != 0:
                    skillPrint += f"{skill.Name}Lv{int(constraints[skill.Name].value())}, "
        print(skillPrint)



        print()
        count += 1
    else:
        break

























































if False:
    if False:
        # 係数行列,個数ベクトル,装備カテゴリ配列
        equip = [[],[],[],[],[],[]]
        numEquip = [[],[],[],[],[],[]]
        equipCategory = ['head', 'body', 'arm', 'wst', 'leg', 'charm']

        # 解の保存用配列
        resEquipIndex = [0] * 6
        resDecoIndex = []


        # スキル条件を設定したCSVファイルを読み込む
        with open('./conditions/skillCondition.csv', mode='r', encoding='utf-8') as file:
            skillCondition = list(csv.reader(file))
        del skillCondition[0]

        # 装備のデータを読み込む
        # 先頭の行(見出し)を削除
        print("データファイルを読み込んでいます...")
        for i in range(len(equipCategory)):
            with open('./data/' + equipCategory[i] + '.csv', mode='r', encoding='utf-8') as file:
                equip[i] = list(csv.reader(file))
            del equip[i][0]
        with open('./data/deco.csv', mode='r', encoding='utf-8') as file:
            deco = list(csv.reader(file))

        with open('./conditions/excludeArmor.csv', mode='r', encoding='utf-8') as file:
            excludeArmor = list(csv.reader(file))

        with open('./conditions/excludeCharm.csv', mode='r', encoding='utf-8') as file:
            excludeCharm = list(csv.reader(file))

        with open('./conditions/haveDeco.csv', mode='r', encoding='utf-8') as file:
            haveDeco = list(csv.reader(file))

        with open('./conditions/weapon.csv', mode='r', encoding='utf-8') as file:
            weapon = list(csv.reader(file))
        del deco[0],excludeArmor[0],haveDeco[0],weapon[0]

        # 個数ベクトルを初期化
        for i in range(len(equipCategory)): numEquip[i] = [0] * len(equip[i])
        numDeco = [0] * (len(deco))

        # 個数ベクトルの定義
        # 防具・護石
        for i in range(len(equipCategory)):
            k = 0
            for j in range(len(numEquip[i])):
                # 防具
                if i < 5:
                    # 存在しない装備はスキップ
                    while int(excludeArmor[j+k][i+1]) == -1: k += 1
                    # 除外しない場合
                    if int(excludeArmor[j+k][i+1]) == 0:
                        numEquip[i][j] = pulp.LpVariable('numEquip'+str(i)+','+str(j), cat=pulp.LpBinary)
                    # 除外する場合
                    elif int(excludeArmor[j+k][i+1]) == 1:
                        numEquip[i][j] = pulp.LpVariable('numEquip'+str(i)+','+str(j), 
                                                        lowBound=0, upBound=0, cat=pulp.LpInteger)
                    else: exit("excludeArmor.csvに0または1以外の文字が含まれています。")
                # 護石
                else:
                    # 除外しない場合
                    if int(excludeCharm[j+k][1]) == 0:
                        numEquip[i][j] = pulp.LpVariable('numEquip'+str(i)+','+str(j), cat=pulp.LpBinary)
                    # 除外する場合
                    elif int(excludeCharm[j+k][1]) == 1:
                        numEquip[i][j] = pulp.LpVariable('numEquip'+str(i)+','+str(j), 
                                                        lowBound=0, upBound=0, cat=pulp.LpInteger)
                    else: exit("excludeCharm.csvに0または1以外の文字が含まれています。")

        # 装飾品
        for i in range(len(deco)):
            numDeco[i] = pulp.LpVariable('numDeco'+str(i), lowBound=0, upBound=int(haveDeco[i][1]), cat=pulp.LpInteger)


        # 結果ベクトル(=係数行列*個数ベクトルの積)の初期化
        y = [0] * (len(equip[0][0])-1)

        # 結果ベクトルの計算
        # 防具・護石
        for i in range(len(y)):
            for j in range(len(equipCategory)):
                for k in range(len(equip[j])):
                    y[i] += int(equip[j][k][i+1]) * numEquip[j][k]
        # 装飾品
        for i in range(len(y)):
            for j in range(len(deco)):
                # print(deco[j][i+1])
                y[i] += int(deco[j][i+1]) * numDeco[j]
        # 武器
        for i in  range(len(y)): y[i] += int(weapon[0][i+1])


        # 防御を最適化
        problem += y[10]

        # ここに制約条件を書く
        # 防具・護石は1個まで装備可能
        for i in range(6): problem += 0 <= y[i] <= 1
        # 装飾品スロットは不足してはいけない
        for i in range(4): problem += 0 <= y[i+6]

        # スキル条件
        # シリーズスキルの極意系発動による上限解放がされていない場合はやり直し
        for i in range(len(skillCondition)):
            if (int(skillCondition[i][4]) == 0) & (int(skillCondition[i][1]) > int(skillCondition[i][2])):
                if not MHSSpy.utl.judgeSecret(i,skillCondition):
                    exit(str(skillCondition[i][0]) + "・極意が指定されていません。対応するシリーズスキルを指定してください。")
            problem += int(skillCondition[i][1]) <= y[i+16], 'skill'+str(i)

        # 検索条件を表示
        # 武器スロ・スキル
        weaponSlot = [0,0,0]
        weaponSkill = ""
        for i in range(7,11):
            for j in range(int(weapon[0][i])):
                weaponSlot[j] += 1
        for i in range(len(skillCondition)):
            if (int(weapon[0][i+1+16]) != 0):
                weaponSkill += skillCondition[i][0] + "Lv" + weapon[0][i+1+16] + ", "
        if weaponSkill == "": weaponSkill = "なし"
        print("武器スロット・スキル: " + str(weaponSlot) + ", " + weaponSkill)
        # スキル
        skill = ""
        for i in range(len(skillCondition)):
            if (int(skillCondition[i][1]) != 0):
                skill += skillCondition[i][0] + "Lv" + skillCondition[i][1] + ", "
        print("検索スキル: " + skill)



    # 最適化問題を解く
    # 検索回数
    # 非負整数以外は弾く
    while (True):
        numSearch = input("検索回数を入力してください。(最大50) ")
        if numSearch.isdecimal(): break
        print("非負整数を入力してください。")
    count = 0
    while (True):
        status = problem.solve(pulp.PULP_CBC_CMD(msg = False))

        # 解があるand検索回数が指定値以下なら解を表示
        if (pulp.LpStatus[status] == "Optimal") & (count < min(int(numSearch),50)):
            # 防具・護石の表示
            for i in range(6):
                for j in range(len(numEquip[i])):
                    if numEquip[i][j].value() != 0:
                        print(equip[i][j][0])
                        resEquipIndex[i] = j
            # 防御力,属性耐性の表示
            for i in range(10,16): print(int(y[i].value()), end=', ')
            print()

            # 発動スキルの表示
            for i in range(16,len(y)):
                if y[i].value() != 0:
                    print(str(skillCondition[i-16][0]) + 'Lv' + str(int(y[i].value())), end=", ")
            print()
            # 必要な装飾品の表示
            for i in range(len(numDeco)):
                if numDeco[i].value() != 0:
                    print(deco[i][0] + "*" + str(int(numDeco[i].value())), end=", ")
                    resDecoIndex.append(i)

            # 余りスロットの表示
            y6 = int(y[6].value())
            y7 = int(y[7].value())
            y8 = int(y[8].value())
            y9 = int(y[9].value())
            print("\n余りスロット(Lv1,2,3,4): ", end="")
            print(str(y6 - min(y6,y7)) + "," + str(min(y6,y7) - min(y6,y7,y8)) + "," +
                str(min(y6,y7,y8) - min(y6,y7,y8,y9)) + "," + str(min(y6,y7,y8,y9)) + "\n")

            # 複数検索のための条件として同じ防具・護石の組み合わせを除外する
            resExcluded = 0
            for i in range(6): resExcluded += numEquip[i][resEquipIndex[i]]
            problem += resExcluded <= 5, 'multiSearch'+str(count)

            count += 1
        else:
            break
    print(str(count) + "件の検索結果")

    # 追加スキル検索
    if count > 0:
        # print("\n追加スキル検索を実行しますか? y/n")
        input = input("\n追加スキル検索を実行しますか? y/n\n")
        if input == 'y':
            try:
                # 複数検索条件をすべて削除
                for i in range(count): del problem.constraints['multiSearch'+str(i)]
                addSkill = []

                # 全スキルから一つだけLvを+1して問題を解く
                for i in range(len(skillCondition)):
                    j = 1
                    print("\r追加スキル検索 (" + str(i+1) + "/" + str(len(skillCondition)) + ")",end="")

                    # 条件を元に戻す
                    if (i != 0):
                        del problem.constraints['skill'+str(i-1)]
                        problem += (int(skillCondition[i-1][1])) <= y[i-1+16], 'skill'+str(i-1)
                    while (True):
                        # シリーズスキルの場合
                        if (int(skillCondition[i][4]) == 1):
                            del problem.constraints['skill'+str(i)]
                            if int(skillCondition[i][1])+j <= int(skillCondition[i][2]):
                                problem += (int(skillCondition[i][2])) <= y[i+16], 'skill'+str(i)
                            elif int(skillCondition[i][1])+j <= int(skillCondition[i][3]):
                                problem += (int(skillCondition[i][3])) <= y[i+16], 'skill'+str(i)
                            else:
                                problem += (int(skillCondition[i][1])+j) <= y[i+16], 'skill'+str(i)
                                break
                        # シリーズスキルではない場合
                        else:
                            # 上限2を超えている or (上限1を超えている&極意非対応スキル)の場合
                            if ((int(skillCondition[i][1])+j > int(skillCondition[i][3])) |
                                ((not MHSSpy.utl.judgeSecret(i,skillCondition)) & 
                                (int(skillCondition[i][1])+j > int(skillCondition[i][2])))):
                                break
                            else:
                                del problem.constraints['skill'+str(i)]
                                problem += (int(skillCondition[i][1])+j) <= y[i+16], 'skill'+str(i)

                        status = problem.solve(pulp.PULP_CBC_CMD(msg = False))

                        if pulp.LpStatus[status] == "Optimal":
                            addSkill.append(skillCondition[i][0] + "Lv" + str(int(skillCondition[i][1])+j))
                        j += 1

                print("\n追加スキル検索結果")
                for s in addSkill: print(s)
            except KeyboardInterrupt:
                print("\n追加スキル検索を中断しました。")
                print("追加スキル検索結果")
                for s in addSkill: print(s)
                exit()