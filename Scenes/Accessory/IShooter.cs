﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IShooter
{
    PackedScene Projectile { get; set; }
    bool ShootProjectile();
}

