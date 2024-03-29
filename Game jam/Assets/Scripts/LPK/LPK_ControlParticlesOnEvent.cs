﻿/***************************************************
File:           LPK_ControlParticlesOnEvent.cs
Authors:        Christopher Onorati
Last Updated:   1/29/2019
Last Version:   2.17

Description:
  This component can be used to control the active state
  of a particle system, when an event is received.

This script is a basic and generic implementation of its 
functionality. It is designed for educational purposes and 
aimed at helping beginners.

Copyright 2018-2019, DigiPen Institute of Technology
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* CLASS NAME  : LPK_ControlParticlesOnEvent
* DESCRIPTION : Class to modify properties of a particle system during gameplay.
**/
public class LPK_ControlParticlesOnEvent : LPK_LogicBase
{
    [Header("Component Properties")]

    [Tooltip("Object to modify particle active state when the specified event is received.")]
    [Rename("Target Modify Object")]
    public GameObject m_pTargetModifyObject;

    [Tooltip("Object to modify particle active state when the specified event is received.  Note this will only affect the first object with the tag found.")]
    [TagDropdown]
    public string m_TargetModifyTag;

    [Tooltip("Set how to change the active state of the particle system.")]
    [Rename("Toggle Type")]
    public LPK_ToggleType m_eToggleType;

    [Header("Event Receiving Info")]

    [Tooltip("Which event will trigger this component's action")]
    public LPK_EventList m_EventTrigger = new LPK_EventList();

    /**
    * FUNCTION NAME: OnStart
    * DESCRIPTION  : Sets up what event to listen to particle active state changing.
    * INPUTS       : None
    * OUTPUTS      : None
    **/
    override protected void OnStart()
    {
        InitializeEvent(m_EventTrigger, OnEvent);

        if (m_pTargetModifyObject == null)
        {
            if (!string.IsNullOrEmpty(m_TargetModifyTag))
                m_pTargetModifyObject = GameObject.FindWithTag(m_TargetModifyTag);
            else
                m_pTargetModifyObject = gameObject;
        }
    }

    /**
    * FUNCTION NAME: OnEvent
    * DESCRIPTION  : Performs change to active state of particle system based on user specifications.
    * INPUTS       : data - Event data to parse for validation.
    * OUTPUTS      : None
    **/
    protected override void OnEvent(LPK_EventManager.LPK_EventData data)
    {
        if (!ShouldRespondToEvent(data))
            return;

        if(!m_pTargetModifyObject.GetComponent<ParticleSystem>())
        {
            if (m_bPrintDebug)
                LPK_PrintDebug(this, "No particle system found on target object.");
        }

        ParticleSystem modifyParticles = m_pTargetModifyObject.GetComponent<ParticleSystem>();

        //Modify active state of particles.
        if (m_eToggleType == LPK_ToggleType.ON)
            modifyParticles.Play();
        else if (m_eToggleType == LPK_ToggleType.OFF)
            modifyParticles.Stop();
        else if (m_eToggleType == LPK_ToggleType.TOGGLE)
        {
            if (modifyParticles.isPlaying)
                modifyParticles.Stop();
            else
                modifyParticles.Play();
        }
    }
}
