using UnityEngine;

static public class ColorPlus {

	//http://www.colourlovers.com/blog/2007/07/24/32-common-color-names-for-easy-reference
	//http://www.yellowpipe.com/yis/tools/hex-to-rgb/color-converter.php

	//static public Color Orange =        new Color(255.0f / 255.0f, 153.0f / 255.0f,  51.0f / 255.0f);
	//static public Color ForestGreen =   new Color( 34.0f / 255.0f, 139.0f / 255.0f,  34.0f / 255.0f);
	//static public Color Olive =         new Color(128.0f / 255.0f, 128.0f / 255.0f,   0.0f / 255.0f);
	//static public Color Lime =          new Color(191.0f / 255.0f, 255.0f / 255.0f,   0.0f / 255.0f);
	//static public Color Golden =        new Color(255.0f / 255.0f, 215.0f / 255.0f,   0.0f / 255.0f);
	//static public Color Goldenrod =     new Color(218.0f / 255.0f, 165.0f / 255.0f,  32.0f / 255.0f);
	//static public Color Maroon =        new Color(128.0f / 255.0f,   0.0f / 255.0f,   0.0f / 255.0f);
	//static public Color Crimson =       new Color(220.0f / 255.0f,  20.0f / 255.0f,  60.0f / 255.0f);
	//static public Color Teal =          new Color(  0.0f / 255.0f, 128.0f / 255.0f, 128.0f / 255.0f);
	//static public Color Azure =         new Color(  0.0f / 255.0f, 127.0f / 255.0f, 255.0f / 255.0f);
	//static public Color RoyalBlue =     new Color(  8.0f / 255.0f,  76.0f / 255.0f, 158.0f / 255.0f);
	//static public Color Chartreuse =    new Color(127.0f / 255.0f, 255.0f / 255.0f,   0.0f / 255.0f);
	//static public Color Ivory =         new Color(255.0f / 255.0f, 255.0f / 255.0f, 240.0f / 255.0f);
	//static public Color Beige =         new Color(245.0f / 255.0f, 245.0f / 255.0f, 220.0f / 255.0f);
	//static public Color Silver =        new Color(192.0f / 255.0f, 192.0f / 255.0f, 192.0f / 255.0f);
	//static public Color Coral =         new Color(255.0f / 255.0f, 127.0f / 255.0f,  80.0f / 255.0f);
	//static public Color Salmon =        new Color(250.0f / 255.0f, 128.0f / 255.0f, 114.0f / 255.0f);
	//static public Color Fuchsia =       new Color(255.0f / 255.0f, 119.0f / 255.0f, 255.0f / 255.0f);
	//static public Color Khaki =         new Color(240.0f / 255.0f, 230.0f / 255.0f, 140.0f / 255.0f);
	//static public Color SpringGreen =   new Color(  0.0f / 255.0f, 255.0f / 255.0f, 127.0f / 255.0f);
	//static public Color SteelBlue =     new Color( 70.0f / 255.0f, 130.0f / 255.0f, 180.0f / 255.0f);
	//static public Color Violet =        new Color(238.0f / 255.0f, 130.0f / 255.0f,   0.0f / 238.0f);
	//static public Color Wheat =         new Color(245.0f / 255.0f, 222.0f / 255.0f, 179.0f / 255.0f);
	//static public Color Tan =           new Color(210.0f / 255.0f, 180.0f / 255.0f, 140.0f / 255.0f);
	//static public Color BurlyWood =     new Color(222.0f / 255.0f, 184.0f / 255.0f, 135.0f / 255.0f);
	//static public Color Lavender =      new Color(230.0f / 255.0f, 230.0f / 255.0f, 250.0f / 255.0f);
	//static public Color Mauve =         new Color(224.0f / 255.0f, 176.0f / 255.0f, 255.0f / 255.0f);
	//static public Color Plum =          new Color(132.0f / 255.0f,  49.0f / 255.0f, 121.0f / 255.0f);
	//static public Color SkyBlue =       new Color(135.0f / 255.0f, 206.0f / 255.0f, 235.0f / 255.0f);
	//static public Color Aquamarine =    new Color(127.0f / 255.0f, 255.0f / 255.0f, 212.0f / 255.0f);
	//static public Color GhostWhite =    new Color(248.0f / 255.0f, 248.0f / 255.0f, 255.0f / 255.0f);

	static public Color AliceBlue               = new Color(240.0f / 255.0f, 248.0f / 255.0f, 255.0f / 255.0f);
	static public Color AntiqueWhite            = new Color(250.0f / 255.0f, 235.0f / 255.0f, 215.0f / 255.0f);
	static public Color Aqua                    = new Color(  0.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
	static public Color Aquamarine              = new Color(127.0f / 255.0f, 255.0f / 255.0f, 212.0f / 255.0f);
	static public Color Azure                   = new Color(240.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
	static public Color Beige                   = new Color(245.0f / 255.0f, 245.0f / 255.0f, 220.0f / 255.0f);
	static public Color Bisque                  = new Color(255.0f / 255.0f, 228.0f / 255.0f, 196.0f / 255.0f);
	static public Color Black                   = new Color(  0.0f / 255.0f,   0.0f / 255.0f,   0.0f / 255.0f);
	static public Color Blanchedalmond          = new Color(255.0f / 255.0f, 235.0f / 255.0f, 205.0f / 255.0f);
	static public Color Blue                    = new Color(  0.0f / 255.0f,   0.0f / 255.0f, 255.0f / 255.0f);
	static public Color BlueViolet              = new Color(138.0f / 255.0f,  43.0f / 255.0f, 226.0f / 255.0f);
	static public Color Brown                   = new Color(165.0f / 255.0f,  42.0f / 255.0f,  42.0f / 255.0f);
	static public Color BurlyWood               = new Color(222.0f / 255.0f, 184.0f / 255.0f, 135.0f / 255.0f);
	static public Color CadetBlue               = new Color( 95.0f / 255.0f, 158.0f / 255.0f, 160.0f / 255.0f);
	static public Color Chartreuse              = new Color(127.0f / 255.0f, 255.0f / 255.0f,   0.0f / 255.0f);
	static public Color Chocolate               = new Color(210.0f / 255.0f, 105.0f / 255.0f,  30.0f / 255.0f);
	static public Color Coral                   = new Color(255.0f / 255.0f, 127.0f / 255.0f,  80.0f / 255.0f);
	static public Color CornflowerBlue          = new Color(100.0f / 255.0f, 149.0f / 255.0f, 237.0f / 255.0f);
	static public Color Cornsilk                = new Color(255.0f / 255.0f, 248.0f / 255.0f, 220.0f / 255.0f);
	static public Color Crimson                 = new Color(220.0f / 255.0f,  20.0f / 255.0f,  60.0f / 255.0f);
	static public Color Cyan                    = new Color(  0.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
	static public Color DarkBlue                = new Color(  0.0f / 255.0f,   0.0f / 255.0f, 139.0f / 255.0f);
	static public Color DarkCyan                = new Color(  0.0f / 255.0f, 139.0f / 255.0f, 139.0f / 255.0f);
	static public Color DarkGoldenrod           = new Color(184.0f / 255.0f, 134.0f / 255.0f,  11.0f / 255.0f);
	static public Color DarkGray                = new Color(169.0f / 255.0f, 169.0f / 255.0f, 169.0f / 255.0f);
	static public Color DarkGreen               = new Color(  0.0f / 255.0f, 100.0f / 255.0f,   0.0f / 255.0f);
	static public Color DarkGrey                = new Color(169.0f / 255.0f, 169.0f / 255.0f, 169.0f / 255.0f);
	static public Color DarkKhaki               = new Color(189.0f / 255.0f, 183.0f / 255.0f, 107.0f / 255.0f);
	static public Color DarkMagenta             = new Color(139.0f / 255.0f,   0.0f / 255.0f, 139.0f / 255.0f);
	static public Color DarkOliveGreen          = new Color( 85.0f / 255.0f, 107.0f / 255.0f,  47.0f / 255.0f);
	static public Color DarkOrange              = new Color(255.0f / 255.0f, 140.0f / 255.0f,   0.0f / 255.0f);
	static public Color DarkOrchid              = new Color(153.0f / 255.0f,  50.0f / 255.0f, 204.0f / 255.0f);
	static public Color DarkRed                 = new Color(139.0f / 255.0f,   0.0f / 255.0f,   0.0f / 255.0f);
	static public Color DarkSalmon              = new Color(233.0f / 255.0f, 150.0f / 255.0f, 122.0f / 255.0f);
	static public Color DarkSeaGreen            = new Color(143.0f / 255.0f, 188.0f / 255.0f, 143.0f / 255.0f);
	static public Color DarkSlateBlue           = new Color( 72.0f / 255.0f,  61.0f / 255.0f, 139.0f / 255.0f);
	static public Color DarkSlateGray           = new Color( 47.0f / 255.0f,  79.0f / 255.0f,  79.0f / 255.0f);
	static public Color DarkSlateGrey           = new Color( 47.0f / 255.0f,  79.0f / 255.0f,  79.0f / 255.0f);
	static public Color DarkTurquoise           = new Color(  0.0f / 255.0f, 206.0f / 255.0f, 209.0f / 255.0f);
	static public Color DarkViolet              = new Color(148.0f / 255.0f,   0.0f / 255.0f, 211.0f / 255.0f);
	static public Color DeepPink                = new Color(255.0f / 255.0f,  20.0f / 255.0f, 147.0f / 255.0f);
	static public Color DeepSkyBlue             = new Color(  0.0f / 255.0f, 191.0f / 255.0f, 255.0f / 255.0f);
	static public Color DimGray                 = new Color(105.0f / 255.0f, 105.0f / 255.0f, 105.0f / 255.0f);
	static public Color DimGrey                 = new Color(105.0f / 255.0f, 105.0f / 255.0f, 105.0f / 255.0f);
	static public Color DodgerBlue              = new Color( 30.0f / 255.0f, 144.0f / 255.0f, 255.0f / 255.0f);
	static public Color FireBrick               = new Color(178.0f / 255.0f,  34.0f / 255.0f,  34.0f / 255.0f);
	static public Color FloralWhite             = new Color(255.0f / 255.0f, 250.0f / 255.0f, 240.0f / 255.0f);
	static public Color ForestGreen             = new Color( 34.0f / 255.0f, 139.0f / 255.0f,  34.0f / 255.0f);
	static public Color Fuchsia                 = new Color(255.0f / 255.0f,   0.0f / 255.0f, 255.0f / 255.0f);
	static public Color Gainsboro               = new Color(220.0f / 255.0f, 220.0f / 255.0f, 220.0f / 255.0f);
	static public Color GhostWhite              = new Color(248.0f / 255.0f, 248.0f / 255.0f, 255.0f / 255.0f);
	static public Color Golden                  = new Color(255.0f / 255.0f, 215.0f / 255.0f,   0.0f / 255.0f);
	static public Color Goldenrod               = new Color(218.0f / 255.0f, 165.0f / 255.0f,  32.0f / 255.0f);
	static public Color Gray                    = new Color(128.0f / 255.0f, 128.0f / 255.0f, 128.0f / 255.0f);
	static public Color Green                   = new Color(  0.0f / 255.0f, 128.0f / 255.0f,   0.0f / 255.0f);
	static public Color GreenYellow             = new Color(173.0f / 255.0f, 255.0f / 255.0f,  47.0f / 255.0f);
	static public Color Grey                    = new Color(128.0f / 255.0f, 128.0f / 255.0f, 128.0f / 255.0f);
	static public Color Honeydew                = new Color(240.0f / 255.0f, 255.0f / 255.0f, 240.0f / 255.0f);
	static public Color Hotpink                 = new Color(255.0f / 255.0f, 105.0f / 255.0f, 180.0f / 255.0f);
	static public Color IndianRed               = new Color(205.0f / 255.0f,  92.0f / 255.0f,  92.0f / 255.0f);
	static public Color Indigo                  = new Color( 75.0f / 255.0f,   0.0f / 255.0f, 130.0f / 255.0f);
	static public Color Ivory                   = new Color(255.0f / 255.0f, 255.0f / 255.0f, 240.0f / 255.0f);
	static public Color Khaki                   = new Color(240.0f / 255.0f, 230.0f / 255.0f, 140.0f / 255.0f);
	static public Color Lavender                = new Color(230.0f / 255.0f, 230.0f / 255.0f, 250.0f / 255.0f);
	static public Color LavenderBlush           = new Color(255.0f / 255.0f, 240.0f / 255.0f, 245.0f / 255.0f);
	static public Color Lawngreen               = new Color(124.0f / 255.0f, 252.0f / 255.0f,   0.0f / 255.0f);
	static public Color Lemonchiffon            = new Color(255.0f / 255.0f, 250.0f / 255.0f, 205.0f / 255.0f);
	static public Color LightBlue               = new Color(173.0f / 255.0f, 216.0f / 255.0f, 230.0f / 255.0f);
	static public Color LightCoral              = new Color(240.0f / 255.0f, 128.0f / 255.0f, 128.0f / 255.0f);
	static public Color LightCyan               = new Color(224.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
	static public Color LightGoldenrodYellow    = new Color(250.0f / 255.0f, 250.0f / 255.0f, 210.0f / 255.0f);
	static public Color LightGray               = new Color(211.0f / 255.0f, 211.0f / 255.0f, 211.0f / 255.0f);
	static public Color LightGreen              = new Color(144.0f / 255.0f, 238.0f / 255.0f, 144.0f / 255.0f);
	static public Color LightGrey               = new Color(211.0f / 255.0f, 211.0f / 255.0f, 211.0f / 255.0f);
	static public Color LightPink               = new Color(255.0f / 255.0f, 182.0f / 255.0f, 193.0f / 255.0f);
	static public Color LightSalmon             = new Color(255.0f / 255.0f, 160.0f / 255.0f, 122.0f / 255.0f);
	static public Color LightSeaGreen           = new Color( 32.0f / 255.0f, 178.0f / 255.0f, 170.0f / 255.0f);
	static public Color LightSkyBlue            = new Color(135.0f / 255.0f, 206.0f / 255.0f, 250.0f / 255.0f);
	static public Color LightSlateGray          = new Color(119.0f / 255.0f, 136.0f / 255.0f, 153.0f / 255.0f);
	static public Color LightSlateGrey          = new Color(119.0f / 255.0f, 136.0f / 255.0f, 153.0f / 255.0f);
	static public Color LightSteelBlue          = new Color(176.0f / 255.0f, 196.0f / 255.0f, 222.0f / 255.0f);
	static public Color LightYellow             = new Color(255.0f / 255.0f, 255.0f / 255.0f, 224.0f / 255.0f);
	static public Color Lime                    = new Color(  0.0f / 255.0f, 255.0f / 255.0f,   0.0f / 255.0f);
	static public Color LimeGreen               = new Color( 50.0f / 255.0f, 205.0f / 255.0f,  50.0f / 255.0f);
	static public Color Linen                   = new Color(250.0f / 255.0f, 240.0f / 255.0f, 230.0f / 255.0f);
	static public Color Magenta                 = new Color(255.0f / 255.0f,   0.0f / 255.0f, 255.0f / 255.0f);
	static public Color Maroon                  = new Color(128.0f / 255.0f,   0.0f / 255.0f,   0.0f / 255.0f);
	static public Color Mauve                   = new Color(224.0f / 255.0f, 176.0f / 255.0f, 255.0f / 255.0f);
	static public Color MediumAquamarine        = new Color(102.0f / 255.0f, 205.0f / 255.0f, 170.0f / 255.0f);
	static public Color MediumBlue              = new Color(  0.0f / 255.0f,   0.0f / 255.0f, 205.0f / 255.0f);
	static public Color MediumOrchid            = new Color(186.0f / 255.0f,  85.0f / 255.0f, 211.0f / 255.0f);
	static public Color MediumPurple            = new Color(147.0f / 255.0f, 112.0f / 255.0f, 219.0f / 255.0f);
	static public Color MediumSeaGreen          = new Color( 60.0f / 255.0f, 179.0f / 255.0f, 113.0f / 255.0f);
	static public Color MediumSlateBlue         = new Color(123.0f / 255.0f, 104.0f / 255.0f, 238.0f / 255.0f);
	static public Color MediumSpringGreen       = new Color(  0.0f / 255.0f, 250.0f / 255.0f, 154.0f / 255.0f);
	static public Color MediumTurquoise         = new Color( 72.0f / 255.0f, 209.0f / 255.0f, 204.0f / 255.0f);
	static public Color MediumVioletRed         = new Color(199.0f / 255.0f,  21.0f / 255.0f, 133.0f / 255.0f);
	static public Color MidnightBlue            = new Color( 25.0f / 255.0f,  25.0f / 255.0f, 112.0f / 255.0f);
	static public Color Mintcream               = new Color(245.0f / 255.0f, 255.0f / 255.0f, 250.0f / 255.0f);
	static public Color Mistyrose               = new Color(255.0f / 255.0f, 228.0f / 255.0f, 225.0f / 255.0f);
	static public Color Moccasin                = new Color(255.0f / 255.0f, 228.0f / 255.0f, 181.0f / 255.0f);
	static public Color NavajoWhite             = new Color(255.0f / 255.0f, 222.0f / 255.0f, 173.0f / 255.0f);
	static public Color Navy                    = new Color(  0.0f / 255.0f,   0.0f / 255.0f, 128.0f / 255.0f);
	static public Color Oldlace                 = new Color(253.0f / 255.0f, 245.0f / 255.0f, 230.0f / 255.0f);
	static public Color Olive                   = new Color(128.0f / 255.0f, 128.0f / 255.0f,   0.0f / 255.0f);
	static public Color Olivedrab               = new Color(107.0f / 255.0f, 142.0f / 255.0f,  35.0f / 255.0f);
	static public Color Orange                  = new Color(255.0f / 255.0f, 165.0f / 255.0f,   0.0f / 255.0f);
	static public Color OrangeRed               = new Color(255.0f / 255.0f,  69.0f / 255.0f,   0.0f / 255.0f);
	static public Color Orchid                  = new Color(218.0f / 255.0f, 112.0f / 255.0f, 214.0f / 255.0f);
	static public Color PaleGoldenrod           = new Color(238.0f / 255.0f, 232.0f / 255.0f, 170.0f / 255.0f);
	static public Color PaleGreen               = new Color(152.0f / 255.0f, 251.0f / 255.0f, 152.0f / 255.0f);
	static public Color PaleTurquoise           = new Color(175.0f / 255.0f, 238.0f / 255.0f, 238.0f / 255.0f);
	static public Color PaleVioletred           = new Color(219.0f / 255.0f, 112.0f / 255.0f, 147.0f / 255.0f);
	static public Color PapayaWhip              = new Color(255.0f / 255.0f, 239.0f / 255.0f, 213.0f / 255.0f);
	static public Color PeachPuff               = new Color(255.0f / 255.0f, 218.0f / 255.0f, 185.0f / 255.0f);
	static public Color Peru                    = new Color(205.0f / 255.0f, 133.0f / 255.0f,  63.0f / 255.0f);
	static public Color Pink                    = new Color(255.0f / 255.0f, 192.0f / 255.0f, 203.0f / 255.0f);
	static public Color Plum                    = new Color(221.0f / 255.0f, 160.0f / 255.0f, 221.0f / 255.0f);
	static public Color PowderBlue              = new Color(176.0f / 255.0f, 224.0f / 255.0f, 230.0f / 255.0f);
	static public Color Purple                  = new Color(128.0f / 255.0f,   0.0f / 255.0f, 128.0f / 255.0f);
	static public Color Red                     = new Color(255.0f / 255.0f,   0.0f / 255.0f,   0.0f / 255.0f);
	static public Color RosyBrown               = new Color(188.0f / 255.0f, 143.0f / 255.0f, 143.0f / 255.0f);
	static public Color RoyalBlue               = new Color( 65.0f / 255.0f, 105.0f / 255.0f, 225.0f / 255.0f);
	static public Color SaddleBrown             = new Color(139.0f / 255.0f,  69.0f / 255.0f,  19.0f / 255.0f);
	static public Color Salmon                  = new Color(250.0f / 255.0f, 128.0f / 255.0f, 114.0f / 255.0f);
	static public Color SandyBrown              = new Color(244.0f / 255.0f, 164.0f / 255.0f,  96.0f / 255.0f);
	static public Color SeaGreen                = new Color( 46.0f / 255.0f, 139.0f / 255.0f,  87.0f / 255.0f);
	static public Color Seashell                = new Color(255.0f / 255.0f, 245.0f / 255.0f, 238.0f / 255.0f);
	static public Color Sienna                  = new Color(160.0f / 255.0f,  82.0f / 255.0f,  45.0f / 255.0f);
	static public Color Silver                  = new Color(192.0f / 255.0f, 192.0f / 255.0f, 192.0f / 255.0f);
	static public Color SkyBlue                 = new Color(135.0f / 255.0f, 206.0f / 255.0f, 235.0f / 255.0f);
	static public Color SlateBlue               = new Color(106.0f / 255.0f,  90.0f / 255.0f, 205.0f / 255.0f);
	static public Color SlateGray               = new Color(112.0f / 255.0f, 128.0f / 255.0f, 144.0f / 255.0f);
	static public Color SlateGrey               = new Color(112.0f / 255.0f, 128.0f / 255.0f, 144.0f / 255.0f);
	static public Color Snow                    = new Color(255.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f);
	static public Color SpringGreen             = new Color(  0.0f / 255.0f, 255.0f / 255.0f, 127.0f / 255.0f);
	static public Color SteelBlue               = new Color( 70.0f / 255.0f, 130.0f / 255.0f, 180.0f / 255.0f);
	static public Color Tan                     = new Color(210.0f / 255.0f, 180.0f / 255.0f, 140.0f / 255.0f);
	static public Color Teal                    = new Color(  0.0f / 255.0f, 128.0f / 255.0f, 128.0f / 255.0f);
	static public Color Thistle                 = new Color(216.0f / 255.0f, 191.0f / 255.0f, 216.0f / 255.0f);
	static public Color Tomato                  = new Color(255.0f / 255.0f,  99.0f / 255.0f,  71.0f / 255.0f);
	static public Color Turquoise               = new Color( 64.0f / 255.0f, 224.0f / 255.0f, 208.0f / 255.0f);
	static public Color Violet                  = new Color(238.0f / 255.0f, 130.0f / 255.0f, 238.0f / 255.0f);
	static public Color Wheat                   = new Color(245.0f / 255.0f, 222.0f / 255.0f, 179.0f / 255.0f);
	static public Color White                   = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
	static public Color Whitesmoke              = new Color(245.0f / 255.0f, 245.0f / 255.0f, 245.0f / 255.0f);
	static public Color Yellow                  = new Color(255.0f / 255.0f, 255.0f / 255.0f,   0.0f / 255.0f);
	static public Color YellowGreen             = new Color(154.0f / 255.0f, 205.0f / 255.0f,  50.0f / 255.0f);

	//For viewer
	static public Color[] Colors = new Color[] {
		AliceBlue,
		AntiqueWhite,
		Aqua,
		Aquamarine,
		Azure,
		Beige,
		Bisque,
		Black,
		Blanchedalmond,
		Blue,
		BlueViolet,
		Brown,
		BurlyWood,
		CadetBlue,
		Chartreuse,
		Chocolate,
		Coral,
		CornflowerBlue,
		Cornsilk,
		Crimson,
		Cyan,
		DarkBlue,
		DarkCyan,
		DarkGoldenrod,
		DarkGray,
		DarkGreen,
		DarkGrey,
		DarkKhaki,
		DarkMagenta,
		DarkOliveGreen,
		DarkOrange,
		DarkOrchid,
		DarkRed,
		DarkSalmon,
		DarkSeaGreen,
		DarkSlateBlue,
		DarkSlateGray,
		DarkSlateGrey,
		DarkTurquoise,
		DarkViolet,
		DeepPink,
		DeepSkyBlue,
		DimGray,
		DimGrey,
		DodgerBlue,
		FireBrick,
		FloralWhite,
		ForestGreen,
		Fuchsia,
		Gainsboro,
		GhostWhite,
		Golden,
		Goldenrod,
		Gray,
		Green,
		GreenYellow,
		Grey,
		Honeydew,
		Hotpink,
		IndianRed,
		Indigo,
		Ivory,
		Khaki,
		Lavender,
		LavenderBlush,
		Lawngreen,
		Lemonchiffon,
		LightBlue,
		LightCoral,
		LightCyan,
		LightGoldenrodYellow,
		LightGray,
		LightGreen,
		LightGrey,
		LightPink,
		LightSalmon,
		LightSeaGreen,
		LightSkyBlue,
		LightSlateGray,
		LightSlateGrey,
		LightSteelBlue,
		LightYellow,
		Lime,
		LimeGreen,
		Linen,
		Magenta,
		Maroon,
		Mauve,
		MediumAquamarine,
		MediumBlue,
		MediumOrchid,
		MediumPurple,
		MediumSeaGreen,
		MediumSlateBlue,
		MediumSpringGreen,
		MediumTurquoise,
		MediumVioletRed,
		MidnightBlue,
		Mintcream,
		Mistyrose,
		Moccasin,
		NavajoWhite,
		Navy,
		Oldlace,
		Olive,
		Olivedrab,
		Orange,
		OrangeRed,
		Orchid,
		PaleGoldenrod,
		PaleGreen,
		PaleTurquoise,
		PaleVioletred,
		PapayaWhip,
		PeachPuff,
		Peru,
		Pink,
		Plum,
		PowderBlue,
		Purple,
		Red,
		RosyBrown,
		RoyalBlue,
		SaddleBrown,
		Salmon,
		SandyBrown,
		SeaGreen,
		Seashell,
		Sienna,
		Silver,
		SkyBlue,
		SlateBlue,
		SlateGray,
		SlateGrey,
		Snow,
		SpringGreen,
		SteelBlue,
		Tan,
		Teal,
		Thistle,
		Tomato,
		Turquoise,
		Violet,
		Wheat,
		White,
		Whitesmoke,
		Yellow,
		YellowGreen
	};

	public enum Name {
		AliceBlue,
		AntiqueWhite,
		Aqua,
		Aquamarine,
		Azure,
		Beige,
		Bisque,
		Black,
		Blanchedalmond,
		Blue,
		BlueViolet,
		Brown,
		BurlyWood,
		CadetBlue,
		Chartreuse,
		Chocolate,
		Coral,
		CornflowerBlue,
		Cornsilk,
		Crimson,
		Cyan,
		DarkBlue,
		DarkCyan,
		DarkGoldenrod,
		DarkGray,
		DarkGreen,
		DarkGrey,
		DarkKhaki,
		DarkMagenta,
		DarkOliveGreen,
		DarkOrange,
		DarkOrchid,
		DarkRed,
		DarkSalmon,
		DarkSeaGreen,
		DarkSlateBlue,
		DarkSlateGray,
		DarkSlateGrey,
		DarkTurquoise,
		DarkViolet,
		DeepPink,
		DeepSkyBlue,
		DimGray,
		DimGrey,
		DodgerBlue,
		FireBrick,
		FloralWhite,
		ForestGreen,
		Fuchsia,
		Gainsboro,
		GhostWhite,
		Golden,
		Goldenrod,
		Gray,
		Green,
		GreenYellow,
		Grey,
		Honeydew,
		Hotpink,
		IndianRed,
		Indigo,
		Ivory,
		Khaki,
		Lavender,
		LavenderBlush,
		Lawngreen,
		Lemonchiffon,
		LightBlue,
		LightCoral,
		LightCyan,
		LightGoldenrodYellow,
		LightGray,
		LightGreen,
		LightGrey,
		LightPink,
		LightSalmon,
		LightSeaGreen,
		LightSkyBlue,
		LightSlateGray,
		LightSlateGrey,
		LightSteelBlue,
		LightYellow,
		Lime,
		LimeGreen,
		Linen,
		Magenta,
		Maroon,
		Mauve,
		MediumAquamarine,
		MediumBlue,
		MediumOrchid,
		MediumPurple,
		MediumSeaGreen,
		MediumSlateBlue,
		MediumSpringGreen,
		MediumTurquoise,
		MediumVioletRed,
		MidnightBlue,
		Mintcream,
		Mistyrose,
		Moccasin,
		NavajoWhite,
		Navy,
		Oldlace,
		Olive,
		Olivedrab,
		Orange,
		OrangeRed,
		Orchid,
		PaleGoldenrod,
		PaleGreen,
		PaleTurquoise,
		PaleVioletred,
		PapayaWhip,
		PeachPuff,
		Peru,
		Pink,
		Plum,
		PowderBlue,
		Purple,
		Red,
		RosyBrown,
		RoyalBlue,
		SaddleBrown,
		Salmon,
		SandyBrown,
		SeaGreen,
		Seashell,
		Sienna,
		Silver,
		SkyBlue,
		SlateBlue,
		SlateGray,
		SlateGrey,
		Snow,
		SpringGreen,
		SteelBlue,
		Tan,
		Teal,
		Thistle,
		Tomato,
		Turquoise,
		Violet,
		Wheat,
		White,
		Whitesmoke,
		Yellow,
		YellowGreen
	};

	static public Color ColorByName(Name name) {
		return Colors[(int)name];
	}
}
