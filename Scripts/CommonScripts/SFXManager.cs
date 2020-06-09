using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Tools.Singleton<SFXManager>
{ 
    public AudioSource piece_pick_voice = null; 
    public AudioSource piece_drop_voice = null;
    public AudioSource piece_take_voice = null;
    public AudioSource card_select_voice = null;
    public AudioSource tower_build_voice = null;
    public AudioSource cannon_shoot_voice = null; 
    public AudioSource tower_upgrade_voice = null;
    public AudioSource tower_destroy_voice = null;
    public AudioSource game_finish_voice = null;
    public AudioSource page_change_voice = null; 
    public AudioSource click_voice = null; 
    public AudioSource buy_sell_voice = null;

    public bool isActive = true;
    
    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Buy_Sell()
    {
        if(buy_sell_voice && isActive)
            buy_sell_voice.Play();
    }

    public void Game_Finished()
    {
        if(game_finish_voice && isActive)
            game_finish_voice.Play();
    }

    public void Tower_Build()
    {
        if(tower_build_voice && isActive)
            tower_build_voice.Play();
    }
    
    public void Cannon_Shot()
    {
        if(cannon_shoot_voice && isActive)
            cannon_shoot_voice.Play();
    }

    public void Tower_Upgraded()
    {
        if(tower_upgrade_voice && isActive)
            tower_upgrade_voice.Play();
    }

    public void Tower_Destroyed()
    {
        if(tower_destroy_voice && isActive)
            tower_destroy_voice.Play();
    }

    public void Piece_Picked()
    {
        if(piece_pick_voice && isActive)
            piece_pick_voice.Play();
    }

    public void Piece_Dropped()
    {
        if(piece_drop_voice && isActive)
            piece_drop_voice.Play();
    }

    public void Piece_Took()
    {
        if(piece_take_voice && isActive)
            piece_take_voice.Play();
    }

    public void Card_Selected()
    {
        if(card_select_voice && isActive)
            card_select_voice.Play();
    }

    public void Page_Changed()
    {
        if(page_change_voice && isActive)
            page_change_voice.Play();
    }

    public void Click()
    {
        if(click_voice && isActive)
            click_voice.Play();
    }
}
