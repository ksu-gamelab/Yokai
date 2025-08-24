using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StoryData
{
    public List<CommandSet> items;
}

[Serializable]
public class ChoiceItem
{
    public string text;   // ボタンに表示されるテキスト
    public string label;  // 遷移先のラベル（ストーリー内で一意）
}


[Serializable]
public class CommandSet
{
    public string label = "";
    public List<Command> commands;
}

[Serializable]
public class Command
{
    public string type;

    // --- show_text
    public string character;
    public string text;
    public float text_speed;
    public bool wait_for_click;
    public string text_ui;

    // --- show_character
    public string position;
    public string motion;
    public bool is_speaking;
    public bool replace;

    // --- set_background
    public string background;

    // --- play_bgm
    public string bgm;
    public float bgm_volume;
    public string fade;
    public float fade_time;

    // --- play_se
    public string se;
    public float delay;

    public float se_volume;

    // --- show_effect / hide_effect
    public string effect;
    public string effect_position;
    public float effect_duration;
    public bool loop;
    public string id;

    // --- camera_animation
    public string camera_animation;
    public float camera_duration;

    // --- next
    public string mode;
    public string next_target;

    // --- fade
    public string prefab;
    public bool wait;

    // --- choice
    public List<ChoiceItem> choices;
}
