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
        deco = ""
        for k, v in variables.items():
            if v.value() != 0:
                if "珠" in k:
                    deco += f"{k}*{int(v.value())}, "
                else:
                    print(k)
        print(deco)

        print(f"{int(constraints['DEF'].value())}, {int(constraints['FIRE'].value())}, {int(constraints['WATER'].value())}, "
              f"{int(constraints['THUNDER'].value())}, {int(constraints['ICE'].value())}, {int(constraints['DRAGON'].value())}")
        
        skillPrint = ""
        for skill in skills:
            if type(constraints[skill.Name]) != int:
                if int(constraints[skill.Name].value()) != 0:
                    if ((skill.Category == "シリーズスキル") or (skill.Category == "グループスキル")):
                        if (skill.MaxLevel1 <= constraints[skill.Name].value()):
                            if ((skill.MaxLevel2 != 0) and (skill.MaxLevel2 <= constraints[skill.Name].value())):
                                skillPrint += f"{skill.ActSkillName2}, "
                            else:
                                skillPrint += f"{skill.ActSkillName1}, "
                    else:
                        skillPrint += f"{skill.Name}Lv{int(constraints[skill.Name].value())}, "
        print(skillPrint)

        slot = []
        for i in range(4):
            slot.append(int(constraints['ARMORSLOT'+str(i+1)].value()))
        print(f"Lv1～4空きスロット数 {utl.Slots(slot)}")
        count += 1
    else:
        break

if count > 0:
    input = input("\n追加スキル検索を実行しますか? y/n\n")
    if input == 'y':
        try:
            # # 複数検索条件をすべて削除
            # for i in range(count): del problem.constraints['multiSearch'+str(i)]
            # addSkill = []

            # 全スキルから一つだけLvを+1して問題を解く
            for s in skillCondition:
                if ((s.Category != "シリーズスキル") and (s.Category != "グループスキル")):
                    if ((s.Category != "属性強化") and (s.Category != "弾・矢強化") and (s.Category != "斬れ味") and (s.Category != "武器スキル")):
                        for i in range(1, s.MaxLevel1 - s.Level + 1):
                            problem += (s.Level) + i <= constraints[s.Name], f"{s.Name}Lv{s.Level+i}"
                            status = problem.solve(pulp.PULP_CBC_CMD(msg = False))
                            if pulp.LpStatus[status] == "Optimal":
                                print(f"{s.Name}Lv{s.Level+i}")
                            del problem.constraints[f"{s.Name}Lv{s.Level+i}"]
                else:
                    if (s.Level < s.MaxLevel1):
                        problem += s.MaxLevel1 <= constraints[s.Name], f"{s.Name}Lv{s.MaxLevel1}"
                        status = problem.solve(pulp.PULP_CBC_CMD(msg = False))
                        if pulp.LpStatus[status] == "Optimal":
                            print(f"{s.ActSkillName1}")
                        del problem.constraints[f"{s.Name}Lv{s.MaxLevel1}"]
                    if ((s.MaxLevel2 != 0) and (s.Level < s.MaxLevel2)):
                        problem += s.MaxLevel2 <= constraints[s.Name], f"{s.Name}Lv{s.MaxLevel2}"
                        status = problem.solve(pulp.PULP_CBC_CMD(msg = False))
                        if pulp.LpStatus[status] == "Optimal":
                            print(f"{s.ActSkillName2}")
                        del problem.constraints[f"{s.Name}Lv{s.MaxLevel2}"]

        except KeyboardInterrupt:
            print("\n追加スキル検索を中断しました。")
            exit()