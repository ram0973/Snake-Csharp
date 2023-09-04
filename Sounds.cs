using System;
using System.Media;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Snake;

public static class Sounds
{
    public static readonly SoundPlayer gameOverPlayer = LoadSound("gameover.wav");
    public static readonly SoundPlayer eatApplePlayer = LoadSound("Assets/power.wav");
    private static SoundPlayer LoadSound(string fileName)
    {
        
        SoundPlayer sp = new SoundPlayer($"Assets/{fileName}");
        return sp;
    }
}