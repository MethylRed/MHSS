import csv
from enum import Enum

# 対応する極意発動でTrue
def judgeSecret(i:int, l:list):
    flag = True
    if i == 8: # 火事場
        flag &= ((int(l[184][1]) >= int(l[184][3])) | # 金獅子の闘志+4
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    elif i == 18: # KO
        flag &= ((int(l[132][1]) >= int(l[132][2])) | # 角竜の覇気+3
                 (int(l[188][1]) >= int(l[188][3])) | # 氷牙竜の絶技+3
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    elif i == 25: # 渾身
        flag &= ((int(l[163][1]) >= int(l[163][2])) | # 斬竜の真髄+3
                 (int(l[184][1]) >= int(l[184][2])) | # 金獅子の闘志+2
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    elif i == 44: # スタミナ奪取
        flag &= ((int(l[167][1]) >= int(l[167][2])) | # 恐暴竜の真髄+3
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    elif i == 45: # スリンガー装填数
        flag &= ((int(l[174][1]) >= int(l[174][2])) | # 銀火竜の真髄+2
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    elif i == 46: # 整備
        flag &= ((int(l[155][1]) >= int(l[155][2])) | # 炎妃龍の真髄+3
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    elif i == 47: # 精霊の加護
        flag &= ((int(l[173][1]) >= int(l[173][2])) | # 金火竜の真髄+2
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    elif i == 58: # 力の解放
        flag &= ((int(l[168][1]) >= int(l[168][2])) | # 雷狼竜の真髄+3
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    elif i == 63: #挑戦者
        flag &= ((int(l[164][1]) >= int(l[164][2])) | # 砕竜の真髄+3
                 (int(l[176][1]) >= int(l[176][3])) | # サバイバー+4
                 (int(l[185][1]) >= int(l[185][2])) | # 砕竜の闘志+2
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    elif i == 96: # 砲術
        flag &= ((int(l[123][1]) >= int(l[123][2])) | # 熔山龍の真髄+3
                 (int(l[185][1]) >= int(l[185][3])) | # 砕竜の闘志+4
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    elif i == 98: # ボマー
        flag &= ((int(l[160][1]) >= int(l[160][2])) | # 調査団の錬金術+3
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    elif i == 102: # 満足感
        flag &= ((int(l[165][1]) >= int(l[165][2])) | # 轟竜の真髄+3
                 (int(l[186][1]) >= int(l[186][3])) | # 爛輝龍の真髄+4
                 (int(l[189][1]) >= int(l[189][2])))  # 黒龍の伝説+2
    return flag

class SKill:
    def __init__(self,
                 Name = "",
                 Category="",
                 Level=0,
                 ActSkillName1="",
                 ActSkillName2="",
                 MaxLevel1=0,
                 MaxLevel2=0,
                 IsFixed=False):
        self.Name = Name
        self.Category = Category
        self.Level = Level
        self.ActSkillName1 = ActSkillName1
        self.ActSkillName2 = ActSkillName2
        self.MaxLevel1 = MaxLevel1
        self.MaxLevel2 = MaxLevel2
        self.IsFixed = IsFixed

class Equip:
    def __init__(self,
                 Kind: int,
                 Name: str,
                 Series: str,
                 SlotType: int,
                 Slot1: int,
                 Slot2: int,
                 Slot3: int,
                 Def: int,
                 ResFire: int,
                 ResWater: int,
                 ResThunder: int,
                 ResIce: int,
                 ResDragon: int,
                 IsLock: bool,
                 IsExclude: bool,
                 Skills: list[SKill],
                 HaveCount: int):
        self.Kind = Kind
        self.Name = Name
        self.Series = Series
        self.SlotType = SlotType
        self.Slot1 = Slot1
        self.Slot2 = Slot2
        self.Slot3 = Slot3
        self.Def = Def
        self.ResFire = ResFire
        self.ResWater = ResWater
        self.ResThunder = ResThunder
        self.ResIce = ResIce
        self.ResDragon = ResDragon
        self.IsLock = IsLock
        self.IsExclude = IsExclude
        self.Skills = Skills
        self.HaveCount = HaveCount

class Kind(Enum):
    WEAPON = 0
    HEAD = 1
    BODY = 2
    ARM = 3
    WST = 4
    LEG = 5
    CHARM = 6
    DECO = 7

def LoadSkills(fileName: str):
    skills = []
    with open(fileName, newline='', encoding='utf-8') as csvfile:
        reader = csv.DictReader(csvfile)
        for row in reader:
            skills.append(SKill(row['名前'], row['カテゴリ'], 0, row['発動スキル1'], row['発動スキル2'], int(row['上限1']), int(row['上限2']), False))
    return skills

def LoadEquips(kind: int, fileName):
    equips = []
    with open(fileName, newline='', encoding='utf-8') as csvfile:
        reader = csv.DictReader(csvfile)
        for row in reader:
            skills = []
            for i in range(5):
                if row['スキル系統'+str(i+1)].strip() == "":
                    break
                else:
                    skills.append(SKill(Name=row['スキル系統'+str(i+1)], Level=int(row['スキル値'+str(i+1)])))

            equips.append(Equip(kind, row['名前'], row['シリーズ名'], 1, int(row['スロット1']), int(row['スロット2']), int(row['スロット3']),
                                int(row['最終防御力']), int(row['火耐性']), int(row['水耐性']), int(row['雷耐性']), int(row['氷耐性']), int(row['龍耐性']),
                                False, False, skills, 0))
    return equips

def LoadCharm(kind: int, fileName):
    equips = []
    with open(fileName, newline='', encoding='utf-8') as csvfile:
        reader = csv.DictReader(csvfile)
        for row in reader:
            skills = []
            for i in range(2):
                if row['スキル系統'+str(i+1)].strip() == "":
                    break
                else:
                    skills.append(SKill(Name=row['スキル系統'+str(i+1)], Level=int(row['スキル値'+str(i+1)])))

            equips.append(Equip(kind, row['名前'], "", 0, int(row['スロット1']), int(row['スロット2']), int(row['スロット3']),
                                0, 0, 0, 0, 0, 0,
                                False, False, skills, 0))
    return equips

def LoadDeco(kind: int, fileName):
    equips = []
    with open(fileName, newline='', encoding='utf-8') as csvfile:
        reader = csv.DictReader(csvfile)
        for row in reader:
            skills = []
            for i in range(2):
                if row['スキル系統'+str(i+1)].strip() == "":
                    break
                else:
                    skills.append(SKill(Name=row['スキル系統'+str(i+1)], Level=int(row['スキル値'+str(i+1)])))

            equips.append(Equip(kind, row['名前'], "", int(row['スロットタイプ']), int(row['スロットサイズ']), 0, 0,
                                0, 0, 0, 0, 0, 0,
                                False, False, skills, 5))
    return equips

def SlotCount(equip: Equip):
    slotCount = [0, 0, 0, 0]
    slot = [equip.Slot1, equip.Slot2, equip.Slot3]
    for s in slot:
        for i in range(s):
            if equip.Kind == Kind.DECO:
                slotCount[i] -= 1
            else:
                slotCount[i] += 1

    return slotCount

def LoadSkillsCondition(fileName: str):
    skills = []
    with open(fileName, newline='', encoding='utf-8') as csvfile:
        reader = csv.DictReader(csvfile)
        for row in reader:
            skills.append(SKill(row['名前'], row['カテゴリ'], int(row['レベル']), row['発動スキル1'], row['発動スキル2'], int(row['上限1']), int(row['上限2']), bool(int(row['固定']))))
    return skills


def Slots(slotCount: list[int]):
    d = [0] * 4
    d[3] = slotCount[3]
    d[2] = slotCount[2] - d[3]
    d[1] = slotCount[1] - d[3] - d[2]
    d[0] = slotCount[0] - d[3] - d[2] - d[1]
    return d

