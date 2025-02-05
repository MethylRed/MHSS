﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class Master
    {
        public static List<Skill> Skills { get; set; } = new();

        public static List<Equip> Weapon { get; set; } = new();

        public static List<Equip> Head { get; set; } = new();

        public static List<Equip> Body { get; set; } = new();
        
        public static List<Equip> Arm { get; set; } = new();
        
        public static List<Equip> Waist { get; set; } = new();
        
        public static List<Equip> Leg { get; set; } = new();

        public static List<Equip> Charm { get; set; } = new();
        
        public static List<Equip> Deco { get; set; } = new();
    }
}
