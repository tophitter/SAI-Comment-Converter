﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;

namespace SAI_Comment_Converter
{
    class Program
    {
        private static readonly Dictionary<SmartEvent, string> smartEventStrings = new Dictionary<SmartEvent, string>();
        private static readonly Dictionary<SmartAction, string> smartActionStrings = new Dictionary<SmartAction, string>();
        private static int totalSkippedScripts, totalLoadedScripts;

        static void Main(string[] args)
        {
            smartEventStrings.Add(SmartEvent.SMART_EVENT_SPELLHIT, "On Spellhit '_spellNameEventParamOne_'");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_HAS_AURA, "On _hasAuraEventParamOne_ '_spellNameEventParamOne_'");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_TARGET_BUFFED, "On Target Buffed With '_spellNameEventParamOne_'");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_SPELLHIT_TARGET, "On Target Spellhit '_spellNameEventParamOne_'");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_FRIENDLY_MISSING_BUFF, "On Friendly Unit Missing Buff '_spellNameEventParamOne_'");

            smartEventStrings.Add(SmartEvent.SMART_EVENT_LINK, "_previousLineComment_");

            smartEventStrings.Add(SmartEvent.SMART_EVENT_HEALT_PCT, "Between _eventParamOne_-_eventParamTwo_% Health");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_MANA_PCT, "Between _eventParamOne_-_eventParamTwo_% Mana");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_RANGE, "Within _eventParamOne_-_eventParamTwo_ Range");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_OOC_LOS, "Within _eventParamOne_-_eventParamTwo_ Range Out of Combat LoS");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_TARGET_HEALTH_PCT, "Target Between _eventParamOne_-_eventParamTwo_% Health");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_FRIENDLY_HEALTH, "Friendly At _eventParamOne_ Health");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_TARGET_MANA_PCT, "Target Between _eventParamOne_-_eventParamTwo_% Mana");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_RECEIVE_EMOTE, "Received Emote _eventParamOne_");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_DAMAGED, "On Damaged Between _eventParamOne_-_eventParamTwo_");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_DAMAGED_TARGET, "On Target Damaged Between _eventParamOne_-_eventParamTwo_");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_MOVEMENTINFORM, "On Reached Point _eventParamTwo_");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_SUMMON_DESPAWNED, "On Summon _npcNameFirstParam_ Despawned");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_DATA_SET, "On Data Set _eventParamOne_ _eventParamTwo_");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_WAYPOINT_REACHED, "On Waypoint _eventParamOne_ Reached");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_TEXT_OVER, "On Text _eventParamOne_ Over");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_RECEIVE_HEAL, "On Received Heal Between _eventParamOne_-_eventParamTwo_");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_TIMED_EVENT_TRIGGERED, "On Timed Event _eventParamOne_ Triggered");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_GOSSIP_SELECT, "On Gossip Option _eventParamTwo_ Selected");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_GAME_EVENT_START, "On Game Event _eventParamOne_ Started");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_GAME_EVENT_END, "On Game Event _eventParamOne_ Ended");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_GO_EVENT_INFORM, "On Event _eventParamOne_ Inform");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_ACTION_DONE, "On Action _eventParamOne_ Done");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_FRIENDLY_HEALTH_PCT, "On Friendly Between _eventParamOne_-_eventParamTwo_% Health");

            smartEventStrings.Add(SmartEvent.SMART_EVENT_UPDATE_IC, "In Combat");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_UPDATE_OOC, "Out of Combat");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_AGGRO, "On Aggro");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_KILL, "On Killed Unit");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_DEATH, "On Just Died");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_EVADE, "On Evade");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_RESPAWN, "On Respawn");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_TARGET_CASTING, "Target Casting '_targetCastingSpellName_'");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_FRIENDLY_IS_CC, "Friendly Crowd Controlled");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_SUMMONED_UNIT, "On Summoned Unit");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_ACCEPTED_QUEST, "On Quest '_questNameEventParamOne_' Taken");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_REWARD_QUEST, "On Quest '_questNameEventParamOne_' Finished");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_REACHED_HOME, "On Reached Home");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_RESET, "On Reset");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_IC_LOS, "In Combat LoS");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_PASSENGER_BOARDED, "On Passenger Boarded");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_PASSENGER_REMOVED, "On Passenger Removed");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_CHARMED, "On Charmed");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_CHARMED_TARGET, "On Target Charmed");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_CORPSE_REMOVED, "On Corpse Removed");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_AI_INIT, "On Initialize");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_WAYPOINT_START, "On Waypoint Started");
            //smartEventStrings.Add(SmartEvent.SMART_EVENT_TRANSPORT_ADDPLAYER, "");
            //smartEventStrings.Add(SmartEvent.SMART_EVENT_TRANSPORT_ADDCREATURE, "");
            //smartEventStrings.Add(SmartEvent.SMART_EVENT_TRANSPORT_REMOVE_PLAYER, "");
            //smartEventStrings.Add(SmartEvent.SMART_EVENT_TRANSPORT_RELOCATE, "");
            //smartEventStrings.Add(SmartEvent.SMART_EVENT_INSTANCE_PLAYER_ENTER, "");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_AREATRIGGER_ONTRIGGER, "On Trigger");
            //smartEventStrings.Add(SmartEvent.SMART_EVENT_QUEST_ACCEPTED, "");
            //smartEventStrings.Add(SmartEvent.SMART_EVENT_QUEST_OBJ_COPLETETION, "");
            //smartEventStrings.Add(SmartEvent.SMART_EVENT_QUEST_COMPLETION, "");
            //smartEventStrings.Add(SmartEvent.SMART_EVENT_QUEST_REWARDED, "");
            //smartEventStrings.Add(SmartEvent.SMART_EVENT_QUEST_FAIL, "");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_JUST_SUMMONED, "On Just Summoned");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_WAYPOINT_PAUSED, "On Waypoint Paused");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_WAYPOINT_RESUMED, "On Waypoint Resumed");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_WAYPOINT_STOPPED, "On Waypoint Stopped");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_WAYPOINT_ENDED, "On Waypoint Finished");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_UPDATE, "On Update");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_JUST_CREATED, "On Just Created");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_GOSSIP_HELLO, "On Gossip Hello");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_FOLLOW_COMPLETED, "On Follow Complete");
            //smartEventStrings.Add(SmartEvent.SMART_EVENT_DUMMY_EFFECT, "");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_IS_BEHIND_TARGET, "On Behind Target");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_GO_STATE_CHANGED, "On Gameobject State Changed");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_ON_SPELLCLICK, "On Spellclick");

            //! Filling up actions
            smartActionStrings.Add(SmartAction.SMART_ACTION_NONE, "Incorrect Action");
            smartActionStrings.Add(SmartAction.SMART_ACTION_TALK, "Say Line _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_FACTION, "Set Faction _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL, "SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL"); // todo dbc (SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL too)
            smartActionStrings.Add(SmartAction.SMART_ACTION_SOUND, "Play Sound _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_EMOTE, "Play Emote _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_FAIL_QUEST, "Fail Quest '_questNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_QUEST, "Add Quest '_questNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_REACT_STATE, "Set Reactstate _reactStateParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ACTIVATE_GOBJECT, "Activate Gameobject");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RANDOM_EMOTE, "Play Random Emote (_actionRandomParameters_)");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CAST, "Cast '_spellNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SUMMON_CREATURE, "Summon Creature '_creatureNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_THREAT_SINGLE_PCT, "Set Single Threat _actionParamOne_-_actionParamTwo_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_THREAT_ALL_PCT, "Set All Threat _actionParamOne_-_actionParamTwo_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS, "Quest Credit '_questNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_UNUSED_16, "Unused Action Type (16)");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_EMOTE_STATE, "Set Emote State _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_UNIT_FLAG, "Set Flag_getUnitFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_UNIT_FLAG, "Remove Flag_getUnitFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_AUTO_ATTACK, "_startOrStopActionParamOne_ Attacking");
            smartActionStrings.Add(SmartAction.SMART_ACTION_COMBAT_MOVEMENT, "_enableDisableActionParamOne_ Combat Movement");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_EVENT_PHASE, "Set Event Phase _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_INC_EVENT_PHASE, "_incrementOrDecrementActionParamOne_ Phase");
            smartActionStrings.Add(SmartAction.SMART_ACTION_EVADE, "Evade");
            smartActionStrings.Add(SmartAction.SMART_ACTION_FLEE_FOR_ASSIST, "Flee For Assist");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_GROUPEVENTHAPPENS, "Quest Credit '_questNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO, "Quest Credit '_questNameCastCreatureOrGo_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVEAURASFROMSPELL, "Remove Aura '_spellNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_FOLLOW, "Follow _getTargetType_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RANDOM_PHASE, "Set Random Phase(_actionRandomParameters_)");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE, "Set Phase Random Between _actionParamOne_-_actionParamTwo_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RESET_GOBJECT, "Reset Gameobject");
            smartActionStrings.Add(SmartAction.SMART_ACTION_KILLED_MONSTER, "Quest Credit '_questNameKillCredit_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_INST_DATA, "Set Instance Data _actionParamOne_ to _actionParamTwo_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_INST_DATA64, "Set Instance Data _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_UPDATE_TEMPLATE, "Update Template To '_creatureNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_DIE, "Kill Self");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_IN_COMBAT_WITH_ZONE, "Set In Combat With Zone");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_FOR_HELP, "Call For Help");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_SHEATH, "Set Sheath _sheathActionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_FORCE_DESPAWN, "Despawn _forceDespawnActionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_INVINCIBILITY_HP_LEVEL, "Set Invincibility Hp _invincibilityHpActionParamsOneTwo_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL, "SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL"); // todo dbc (SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL too)
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_PHASE_MASK, "Set Phase _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_DATA, "Set Data _actionParamOne_ _actionParamTwo_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_MOVE_FORWARD, "Move Forward _actionParamOne_ Yards");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_VISIBILITY, "Set Visibility _onOffActionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_ACTIVE, "Set Active _onOffActionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ATTACK_START, "Start Attacking");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SUMMON_GO, "Summon Gameobject '_gameobjectNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_KILL_UNIT, "Kill Target");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ACTIVATE_TAXI, "Activate Taxi Path _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_WP_START, "Start Waypoint");
            smartActionStrings.Add(SmartAction.SMART_ACTION_WP_PAUSE, "Pause Waypoint");
            smartActionStrings.Add(SmartAction.SMART_ACTION_WP_STOP, "Stop Waypoint");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_ITEM, "Add Item _addItemBasedOnActionParams_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_ITEM, "Remove Item _addItemBasedOnActionParams_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE, "Install _updateAiTemplateActionParamOne_ Template");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_RUN, "Set Run _onOffActionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_FLY, "Set Fly _onOffActionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_SWIM, "Set Swim _onOffActionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_TELEPORT, "Teleport");
            smartActionStrings.Add(SmartAction.SMART_ACTION_STORE_VARIABLE_DECIMAL, "Store Variables _actionParamOne_ _actionParamTwo_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_STORE_TARGET_LIST, "Store Targetlist");
            smartActionStrings.Add(SmartAction.SMART_ACTION_WP_RESUME, "Resume Waypoint");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_ORIENTATION, "Set Orientation _setOrientationTargetType_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CREATE_TIMED_EVENT, "Create Timed Event");
            smartActionStrings.Add(SmartAction.SMART_ACTION_PLAYMOVIE, "Play Movie _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_MOVE_TO_POS, "Move To _getTargetType_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RESPAWN_TARGET, "Respawn Gameobject");
            smartActionStrings.Add(SmartAction.SMART_ACTION_EQUIP, "Change Equipment");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CLOSE_GOSSIP, "Close Gossip");
            smartActionStrings.Add(SmartAction.SMART_ACTION_TRIGGER_TIMED_EVENT, "Trigger Timed Event _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_TIMED_EVENT, "Remove Timed Event _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_AURA, "Add Aura '_spellNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_OVERRIDE_SCRIPT_BASE_OBJECT, "Override Base Object Script");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RESET_SCRIPT_BASE_OBJECT, "Reset  Base Object Script");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_SCRIPT_RESET, "Reset All Scriptsfa");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_RANGED_MOVEMENT, "Set Ranged Movement");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST, "Run Script");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_NPC_FLAG, "Set Unit Flag_getUnitFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_NPC_FLAG, "Add Unit Flag_getUnitFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_NPC_FLAG, "Remove Unit Flag_getUnitFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SIMPLE_TALK, "Say Line _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_INVOKER_CAST, "Invoker Cast '_spellNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CROSS_CAST, "Cross Cast '_spellNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_RANDOM_TIMED_ACTIONLIST, "Run Random Script");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST, "Run Random Script");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RANDOM_MOVE, "Start Random Movement");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_UNIT_FIELD_BYTES_1, "Set Flag _getBytes1Flags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1, "Remove Flag _getBytes1Flags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_INTERRUPT_SPELL, "Interrupt Spell '_spellNameActionParamTwo_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SEND_GO_CUSTOM_ANIM, "Send Custom Animation _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_DYNAMIC_FLAG, "Set Dynamic Flag_getDynamicFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_DYNAMIC_FLAG, "Add Dynamic Flag_getDynamicFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_DYNAMIC_FLAG, "Remove Dynamic Flag_getDynamicFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_JUMP_TO_POS, "Jump To Pos");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SEND_GOSSIP_MENU, "Send Gossip");
            smartActionStrings.Add(SmartAction.SMART_ACTION_GO_SET_LOOT_STATE, "Set Lootstate _goStateActionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SEND_TARGET_TO_TARGET, "Send Target _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_HOME_POS, "Set Home Position");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_HEALTH_REGEN, "Set Health Regeneration _onOffActionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_ROOT, "Set Rooted _onOffActionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_GO_FLAG, "Set Gameobject Flag_getGoFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_GO_FLAG, "Add Gameobject Flag_getGoFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_GO_FLAG, "Remove Gameobject Flag_getGoFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SUMMON_CREATURE_GROUP, "Summon Creature Group _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_POWER, "Set _powerTypeActionParamOne_ To _actionParamTwo_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_POWER, "Add _actionParamTwo_ _powerTypeActionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_POWER, "Remove _actionParamTwo_ _powerTypeActionParamOne_");

            string host = "127.0.0.1";
            string user = "root";
            string pass = "1234";
            string worldDB = "trinitycore_world";
            string port = "3306";

            Console.WriteLine("SQL Information:");
            //Console.Write("Host: ");
            //string host = Console.ReadLine();
            //Console.Write("User: ");
            //string user = Console.ReadLine();
            //Console.Write("Pass: ");
            //string pass = Console.ReadLine();
            //Console.Write("World DB: ");
            //string worldDB = Console.ReadLine();
            //Console.Write("Port: ");
            //string port = Console.ReadLine();
            
            try
            {
                MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
                connectionString.UserID = user;
                connectionString.Password = pass;
                connectionString.Server = host;
                connectionString.Database = worldDB;
                connectionString.Port = Convert.ToUInt32(port);

                using (var connection = new MySqlConnection(connectionString.ToString()))
                {
                    connection.Open();
                    var returnVal = new MySqlDataAdapter(String.Format("SELECT * FROM smart_scripts ORDER BY entryorguid"), connection);
                    var dataTable = new DataTable();
                    returnVal.Fill(dataTable);

                    if (dataTable.Rows.Count <= 0)
                        return;

                    File.Delete("output.sql");

                    using (var outputFile = new StreamWriter("output.sql", true))
                    {
                        SmartScript smartScriptLink = null;

                        foreach (DataRow row in dataTable.Rows)
                        {
                            SmartScript smartScript = BuildSmartScript(row);

                            totalLoadedScripts++;

                            MySqlCommand command = new MySqlCommand();
                            command.Connection = connection;

                            string fullLine = String.Empty;
                            int entry = smartScript.entryorguid;
                            MySqlDataReader readerSource = null;

                            switch (smartScript.source_type)
                            {
                                case 0: //! Creature
                                    if (smartScript.entryorguid < 0)
                                        entry = GetCreatureIdByGuid(connection, -smartScript.entryorguid);

                                    //! Event type
                                    fullLine += GetCreatureNameByEntry(connection, entry) + " - ";
                                    fullLine += smartEventStrings[(SmartEvent)smartScript.event_type];
                                    break;
                                case 1: //! Gameobject
                                    if (smartScript.entryorguid < 0)
                                        entry = GetGameobjectIdByGuid(connection, -smartScript.entryorguid);

                                    //! Event type
                                    fullLine += GetGameobjectNameByEntry(connection, entry) + " - ";
                                    fullLine += smartEventStrings[(SmartEvent)smartScript.event_type];
                                    break;
                                case 9: //! Actionlist
                                    command.CommandText = (String.Format("SELECT entryorguid, source_type FROM smart_scripts WHERE action_type=80 AND action_param1={0}", smartScript.entryorguid));
                                    readerSource = command.ExecuteReader(CommandBehavior.Default);

                                    if (readerSource.Read())
                                        entry = Convert.ToInt32(readerSource[0]);
                                    else
                                    {
                                        command.CommandText = (String.Format("SELECT entryorguid, source_type FROM smart_scripts WHERE action_type=87 AND (action_param1={0} OR action_param1={1} OR action_param1={2} OR action_param1={3} OR action_param1={4} OR action_param1={5})", smartScript.entryorguid, smartScript.entryorguid, smartScript.entryorguid, smartScript.entryorguid, smartScript.entryorguid, smartScript.entryorguid));
                                        readerSource = command.ExecuteReader(CommandBehavior.Default);

                                        if (readerSource.Read())
                                            entry = Convert.ToInt32(readerSource[0]);
                                        else
                                        {
                                            command.CommandText = ("SELECT action_param1, action_param2 FROM smart_scripts WHERE action_type = 88");
                                            readerSource = command.ExecuteReader(CommandBehavior.Default);

                                            while (readerSource.Read())
                                            {
                                                for (int i = 0; i < readerSource.FieldCount; ++i)
                                                {
                                                    if (Convert.ToInt32(readerSource.GetValue(0)) <= smartScript.entryorguid && Convert.ToInt32(readerSource.GetValue(1)) >= smartScript.entryorguid)
                                                    {
                                                        Console.WriteLine(Convert.ToInt32(readerSource.GetValue(0)));
                                                        Console.WriteLine(Convert.ToInt32(readerSource.GetValue(1)));
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    int source_type = Convert.ToInt32(readerSource[1]);
                                    readerSource.Close();

                                    if (source_type == 0)
                                        fullLine += GetCreatureNameByEntry(connection, entry) + " - On Script";
                                    else if (source_type == 1)
                                        fullLine += GetGameobjectNameByEntry(connection, entry) + " - On Script";
                                    else
                                        fullLine += "<Unknown source_type for actionlist source> - On Script";

                                    break;
                                case 2: //! Areatrigger
                                    continue;
                            }

                            if (fullLine.Contains("_previousLineComment_"))
                            {
                                if (smartScriptLink != null)
                                {
                                    fullLine = fullLine.Replace("_previousLineComment_", smartEventStrings[(SmartEvent)smartScriptLink.event_type]);
                                    smartScript.event_param1 = Convert.ToInt32(smartScriptLink.event_param1);
                                    smartScript.event_param2 = Convert.ToInt32(smartScriptLink.event_param2);
                                    smartScript.event_param3 = Convert.ToInt32(smartScriptLink.event_param3);
                                    smartScript.event_param4 = Convert.ToInt32(smartScriptLink.event_param4);

                                    if (smartScriptLink.link == 0)
                                        smartScriptLink = null;
                                }
                            }
                            else if (smartScript.link > 0)
                                smartScriptLink = smartScript;

                            //! This must be called AFTER we check for _previousLineComment_ so that copied event types don't need special handling
                            if (fullLine.Contains("_eventParamOne_"))
                                fullLine = fullLine.Replace("_eventParamOne_", smartScript.event_param1.ToString());

                            if (fullLine.Contains("_eventParamTwo_"))
                                fullLine = fullLine.Replace("_eventParamTwo_", smartScript.event_param2.ToString());

                            if (fullLine.Contains("_eventParamThree_"))
                                fullLine = fullLine.Replace("_eventParamThree_", smartScript.event_param3.ToString());

                            if (fullLine.Contains("_eventParamFour_"))
                                fullLine = fullLine.Replace("_eventParamFour_", smartScript.event_param4.ToString());

                            if (fullLine.Contains("_spellNameEventParamOne_"))
                            {
                                if (smartScript.event_param1 > 0)
                                {
                                    MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT spellName FROM spells_dbc WHERE id = {0}", smartScript.event_param1), connection);
                                    MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                    if (readerSelect.Read())
                                        fullLine = fullLine.Replace("_spellNameEventParamOne_", readerSelect[0].ToString());
                                    else
                                        fullLine = fullLine.Replace("_spellNameEventParamOne_", "<Spell not found!>");

                                    readerSelect.Close();
                                }
                                else
                                    fullLine = fullLine.Replace(" '_spellNameEventParamOne_'", String.Empty);
                            }

                            if (fullLine.Contains("_targetCastingSpellName_"))
                            {
                                if (smartScript.event_param3.ToString() != "0")
                                {
                                    MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT spellName FROM spells_dbc WHERE id = {0}", smartScript.event_param3), connection);
                                    MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                    if (readerSelect.Read())
                                        fullLine = fullLine.Replace("_targetCastingSpellName_", "'" + readerSelect[0] + "'");
                                    else
                                        fullLine = fullLine.Replace("_targetCastingSpellName_", "<Spell not found!>");

                                    readerSelect.Close();
                                }
                                else
                                    fullLine = fullLine.Replace(" '_targetCastingSpellName_'", String.Empty);
                            }

                            if (fullLine.Contains("_questNameEventParamOne_"))
                            {
                                if (smartScript.event_param1 == 0) //! Any quest (SMART_EVENT_ACCEPTED_QUEST / SMART_EVENT_REWARD_QUEST)
                                    fullLine = fullLine.Replace(" '_questNameEventParamOne_'", String.Empty);
                                else
                                {
                                    MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT title FROM quest_template WHERE id = {0}", smartScript.event_param1), connection);
                                    MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                    if (readerSelect.Read())
                                        fullLine = fullLine.Replace("_questNameEventParamOne_", readerSelect[0].ToString());
                                    else
                                        fullLine = fullLine.Replace("_questNameEventParamOne_", "<Quest not found!>");

                                    readerSelect.Close();
                                }
                            }

                            if (fullLine.Contains("_hasAuraEventParamOne_"))
                                fullLine = fullLine.Replace("_hasAuraEventParamOne_", smartScript.event_param1 > 0 ? "Has Aura" : "Aura Not Present");

                            //! Action type
                            fullLine += " - " + smartActionStrings[(SmartAction)smartScript.action_type];

                            if (fullLine.Contains("_actionParamOne_"))
                                fullLine = fullLine.Replace("_actionParamOne_", smartScript.action_param1.ToString());

                            if (fullLine.Contains("_actionParamTwo_"))
                                fullLine = fullLine.Replace("_actionParamTwo_", smartScript.action_param2.ToString());

                            if (fullLine.Contains("_actionParamThree_"))
                                fullLine = fullLine.Replace("_actionParamThree_", smartScript.action_param3.ToString());

                            if (fullLine.Contains("_actionParamFour_"))
                                fullLine = fullLine.Replace("_actionParamFour_", smartScript.action_param4.ToString());

                            if (fullLine.Contains("_actionParamFive_"))
                                fullLine = fullLine.Replace("_actionParamFive_", smartScript.action_param5.ToString());

                            if (fullLine.Contains("_actionParamSix_"))
                                fullLine = fullLine.Replace("_actionParamSix_", smartScript.action_param6.ToString());

                            if (fullLine.Contains("_spellNameActionParamOne_"))
                            {
                                if (smartScript.action_param1.ToString() != "0")
                                {
                                    MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT spellName FROM spells_dbc WHERE id = {0}", smartScript.action_param1), connection);
                                    MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                    if (readerSelect.Read())
                                        fullLine = fullLine.Replace("_spellNameActionParamOne_", readerSelect[0].ToString());
                                    else
                                        fullLine = fullLine.Replace("_spellNameActionParamOne_", "<Spell not found!>");

                                    readerSelect.Close();
                                }
                                else
                                    fullLine = fullLine.Replace(" '_spellNameActionParamOne_", String.Empty);
                            }

                            if (fullLine.Contains("_spellNameActionParamTwo_"))
                            {
                                if (smartScript.event_param2.ToString() != "0")
                                {
                                    MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT spellName FROM spells_dbc WHERE id = {0}", smartScript.action_param2), connection);
                                    MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                    if (readerSelect.Read())
                                        fullLine = fullLine.Replace("_spellNameActionParamTwo_", readerSelect[0].ToString());
                                    else
                                        fullLine = fullLine.Replace("_spellNameActionParamTwo_", "<Spell not found!>");

                                    readerSelect.Close();
                                }
                                else
                                    fullLine = fullLine.Replace(" '_spellNameActionParamTwo_", String.Empty);
                            }

                            if (fullLine.Contains("_questNameActionParamOne_"))
                            {
                                MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT title FROM quest_template WHERE id = {0}", smartScript.action_param1), connection);
                                MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                if (readerSelect.Read())
                                    fullLine = fullLine.Replace("_questNameActionParamOne_", readerSelect[0].ToString());
                                else
                                    fullLine = fullLine.Replace("_questNameActionParamOne_", "<Quest not found!>");

                                readerSelect.Close();
                            }

                            if (fullLine.Contains("_questNameCastCreatureOrGo_"))
                            {
                                MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT title FROM quest_template WHERE (RequiredNpcOrGo1 = {0} OR RequiredNpcOrGo2 = {1} OR RequiredNpcOrGo3 = {2} OR RequiredNpcOrGo4 = {3}) AND RequiredSpellCast1 = {4}", smartScript.action_param1, smartScript.action_param1, smartScript.action_param1, smartScript.action_param1, smartScript.action_param2), connection);
                                MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                if (readerSelect.Read())
                                    fullLine = fullLine.Replace("_questNameCastCreatureOrGo_", readerSelect[0].ToString());
                                else
                                    fullLine = fullLine.Replace("_questNameCastCreatureOrGo_", "<Quest not found!>");

                                readerSelect.Close();
                            }

                            if (fullLine.Contains("_questNameKillCredit_"))
                            {
                                MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT title FROM quest_template WHERE (RequiredNpcOrGo1 = {0} OR RequiredNpcOrGo2 = {1} OR RequiredNpcOrGo3 = {2} OR RequiredNpcOrGo4 = {3})", smartScript.action_param1, smartScript.action_param1, smartScript.action_param1, smartScript.action_param1), connection);
                                MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                if (readerSelect.Read())
                                    fullLine = fullLine.Replace("_questNameKillCredit_", readerSelect[0].ToString());
                                else
                                    fullLine = fullLine.Replace("_questNameKillCredit_", "<Quest not found!>");

                                readerSelect.Close();
                            }

                            if (fullLine.Contains("_reactStateParamOne_"))
                            {
                                switch (smartScript.action_param1)
                                {
                                    case 0:
                                        fullLine = fullLine.Replace("_reactStateParamOne_", "Passive");
                                        break;
                                    case 1:
                                        fullLine = fullLine.Replace("_reactStateParamOne_", "Defensive");
                                        break;
                                    case 2:
                                        fullLine = fullLine.Replace("_reactStateParamOne_", "Aggressive");
                                        break;
                                    default:
                                        fullLine = fullLine.Replace("_reactStateParamOne_", "<Unknown Reactstate>");
                                        break;
                                }
                            }

                            if (fullLine.Contains("_actionRandomParameters_"))
                            {
                                string randomEmotes = smartScript.action_param1 + ", " + smartScript.action_param2;

                                if (smartScript.action_param3 > 0)
                                    randomEmotes += ", " + smartScript.action_param3;

                                if (smartScript.action_param4 > 0)
                                    randomEmotes += ", " + smartScript.action_param4;

                                if (smartScript.action_param5 > 0)
                                    randomEmotes += ", " + smartScript.action_param5;

                                if (smartScript.action_param6 > 0)
                                    randomEmotes += ", " + smartScript.action_param6;

                                fullLine = fullLine.Replace("_actionRandomParameters_", randomEmotes);
                            }

                            if (fullLine.Contains("_creatureNameActionParamOne_"))
                            {
                                MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT name FROM creature_template WHERE entry = {0}", smartScript.action_param1), connection);
                                MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                if (readerSelect.Read())
                                    fullLine = fullLine.Replace("_creatureNameActionParamOne_", readerSelect[0].ToString());
                                else
                                    fullLine = fullLine.Replace("_creatureNameActionParamOne_", "<_creatureNameActionParamOne_ Creature not found!>");

                                readerSelect.Close();
                            }

                            if (fullLine.Contains("_getUnitFlags_"))
                            {
                                string commentUnitFlag = "";
                                int unitFlags = smartScript.action_param1;

                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_SERVER_CONTROLLED) != 0)  commentUnitFlag += "Server Controlled & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_NON_ATTACKABLE) != 0)     commentUnitFlag += "Not Attackable & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_DISABLE_MOVE) != 0)       commentUnitFlag += "Disable Movement & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PVP_ATTACKABLE) != 0)     commentUnitFlag += "Pvp Attackable & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_RENAME) != 0)             commentUnitFlag += "Rename & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PREPARATION) != 0)        commentUnitFlag += "Preparation & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_NOT_ATTACKABLE_1) != 0)   commentUnitFlag += "Not Attackable & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_IMMUNE_TO_PC) != 0)       commentUnitFlag += "Immune To Players & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_IMMUNE_TO_NPC) != 0)      commentUnitFlag += "Immune To NPC's & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_LOOTING) != 0)            commentUnitFlag += "Looting & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PET_IN_COMBAT) != 0)      commentUnitFlag += "Pet In Combat & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PVP) != 0)                commentUnitFlag += "PvP & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_SILENCED) != 0)           commentUnitFlag += "Silenced & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PACIFIED) != 0)           commentUnitFlag += "Pacified & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_STUNNED) != 0)            commentUnitFlag += "Stunned & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_IN_COMBAT) != 0)          commentUnitFlag += "In Combat & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_DISARMED) != 0)           commentUnitFlag += "Disarmed & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_CONFUSED) != 0)           commentUnitFlag += "Confused & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_FLEEING) != 0)            commentUnitFlag += "Fleeing & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PLAYER_CONTROLLED) != 0)  commentUnitFlag += "Player Controlled & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_NOT_SELECTABLE) != 0)     commentUnitFlag += "Not Selectable & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_SKINNABLE) != 0)          commentUnitFlag += "Skinnable & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_MOUNT) != 0)              commentUnitFlag += "Mounted & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_SHEATHE) != 0)            commentUnitFlag += "Sheathed & ";

                                commentUnitFlag = commentUnitFlag.Trim(new[] { ' ', '&', ' ' }); //! Trim last ' & ' from the comment..

                                if (commentUnitFlag.Contains("&"))
                                    fullLine = fullLine.Replace("_getUnitFlags_", "s_getUnitFlags_");

                                fullLine = fullLine.Replace("_getUnitFlags_", " " + commentUnitFlag);
                            }

                            if (fullLine.Contains("_startOrStopActionParamOne_"))
                            {
                                if (smartScript.action_param1.ToString() == "0")
                                    fullLine = fullLine.Replace("_startOrStopActionParamOne_", "Stop");
                                else //! Even if above 1 or below 0 we start attacking/allow-combat-movement
                                    fullLine = fullLine.Replace("_startOrStopActionParamOne_", "Start");
                            }

                            if (fullLine.Contains("_enableDisableActionParamOne_"))
                            {
                                if (smartScript.action_param1.ToString() == "0")
                                    fullLine = fullLine.Replace("_enableDisableActionParamOne_", "Disable");
                                else //! Even if above 1 or below 0 we start attacking/allow-combat-movement
                                    fullLine = fullLine.Replace("_enableDisableActionParamOne_", "Enable");
                            }

                            if (fullLine.Contains("_incrementOrDecrementActionParamOne_"))
                            {
                                if (smartScript.action_param1.ToString() == "1")
                                    fullLine = fullLine.Replace("_incrementOrDecrementActionParamOne_", "Increment");
                                else if (smartScript.action_param2.ToString() == "1")
                                    fullLine = fullLine.Replace("_incrementOrDecrementActionParamOne_", "Decrement");
                                //else //? What to do?
                            }

                            if (fullLine.Contains("_sheathActionParamOne_"))
                            {
                                switch (smartScript.action_param1)
                                {
                                    case 0:
                                        fullLine = fullLine.Replace("_sheathActionParamOne_", "Unarmed");
                                        break;
                                    case 1:
                                        fullLine = fullLine.Replace("_sheathActionParamOne_", "Melee");
                                        break;
                                    case 2:
                                        fullLine = fullLine.Replace("_sheathActionParamOne_", "Ranged");
                                        break;
                                    default:
                                        fullLine = fullLine.Replace("_sheathActionParamOne_", "<Unknown Sheath>");
                                        break;
                                }
                            }

                            if (fullLine.Contains("_forceDespawnActionParamOne_"))
                            {
                                if (smartScript.action_param1 > 0)
                                    fullLine = fullLine.Replace("_forceDespawnActionParamOne_", "In " + smartScript.action_param1 + " ms");
                                else
                                    fullLine = fullLine.Replace("_forceDespawnActionParamOne_", "Instant");
                            }

                            if (fullLine.Contains("_invincibilityHpActionParamsOneTwo_"))
                            {
                                if (smartScript.action_param1 > 0)
                                    fullLine = fullLine.Replace("_invincibilityHpActionParamsOneTwo_", smartScript.action_param1.ToString());
                                else if (smartScript.action_param2 > 0)
                                    fullLine = fullLine.Replace("_invincibilityHpActionParamsOneTwo_", smartScript.action_param2 + "%");
                                else
                                    fullLine = fullLine.Replace("_invincibilityHpActionParamsOneTwo_", "<Unsupported parameters>");
                            }

                            if (fullLine.Contains("_onOffActionParamOne_"))
                            {
                                if (smartScript.action_param1 == 1)
                                    fullLine = fullLine.Replace("_onOffActionParamOne_", "On");
                                else
                                    fullLine = fullLine.Replace("_onOffActionParamOne_", "Off");
                            }

                            if (fullLine.Contains("_gameobjectNameActionParamOne_"))
                            {
                                MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT name FROM gameobject_template WHERE entry = {0}", smartScript.action_param1), connection);
                                MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                if (readerSelect.Read())
                                    fullLine = fullLine.Replace("_gameobjectNameActionParamOne_", readerSelect[0].ToString());
                                else
                                    fullLine = fullLine.Replace("_gameobjectNameActionParamOne_", "<_gameobjectNameActionParamOne_ Gameobject not found!>");

                                readerSelect.Close();
                            }

                            if (fullLine.Contains("_addItemBasedOnActionParams_"))
                            {
                                MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT name FROM item_template WHERE entry = {0}", smartScript.action_param1), connection);
                                MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                if (readerSelect.Read())
                                {
                                    fullLine = fullLine.Replace("_addItemBasedOnActionParams_", "'" + readerSelect[0] + "' ");

                                    if (smartScript.action_param2 > 1)
                                        fullLine += smartScript.action_param2 + " Times";
                                    else
                                        fullLine += "1 Time";
                                }
                                else
                                    fullLine = fullLine.Replace("_addItemBasedOnActionParams_", "<_addItemBasedOnActionParams_ Item not found!>");

                                readerSelect.Close();
                            }

                            if (fullLine.Contains("_updateAiTemplateActionParamOne_"))
                            {
                                switch ((SmartAiTemplates)smartScript.action_param1)
                                {
                                    case SmartAiTemplates.SMARTAI_TEMPLATE_BASIC:
                                        fullLine = fullLine.Replace("_updateAiTemplateActionParamOne_", "Basic");
                                        break;
                                    case SmartAiTemplates.SMARTAI_TEMPLATE_CASTER:
                                        fullLine = fullLine.Replace("_updateAiTemplateActionParamOne_", "Caster");
                                        break;
                                    case SmartAiTemplates.SMARTAI_TEMPLATE_TURRET:
                                        fullLine = fullLine.Replace("_updateAiTemplateActionParamOne_", "Turret");
                                        break;
                                    case SmartAiTemplates.SMARTAI_TEMPLATE_PASSIVE:
                                        fullLine = fullLine.Replace("_updateAiTemplateActionParamOne_", "Passive");
                                        break;
                                    case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_GO_PART:
                                        fullLine = fullLine.Replace("_updateAiTemplateActionParamOne_", "Caged Gameobject Part");
                                        break;
                                    case SmartAiTemplates.SMARTAI_TEMPLATE_CAGED_NPC_PART:
                                        fullLine = fullLine.Replace("_updateAiTemplateActionParamOne_", "Caged Creature Part");
                                        break;
                                    default:
                                        fullLine = fullLine.Replace("_updateAiTemplateActionParamOne_", "<_updateAiTemplateActionParamOne_ Unknown ai template>");
                                        break;
                                }
                            }

                            if (fullLine.Contains("_setOrientationTargetType_"))
                            {
                                switch ((SmartTargetType)smartScript.target_type)
                                {
                                    case SmartTargetType.SMART_TARGET_SELF:
                                        fullLine = fullLine.Replace("_setOrientationTargetType_", "Home Position");
                                        break;
                                    case SmartTargetType.SMART_TARGET_POSITION:
                                        fullLine = fullLine.Replace("_setOrientationTargetType_", smartScript.target_o.ToString());
                                        break;
                                    default:
                                        fullLine = fullLine.Replace("_setOrientationTargetType_", "Target");
                                        break;
                                }
                            }

                            if (fullLine.Contains("_getTargetType_"))
                                fullLine = fullLine.Replace("_getTargetType_", GetStringByTargetType(smartScript, connection));

                            if (fullLine.Contains("_goStateActionParamOne_"))
                            {
                                switch (smartScript.action_param1)
                                {
                                    case 0:
                                        fullLine = fullLine.Replace("_goStateActionParamOne_", "Not Ready");
                                        break;
                                    case 1:
                                        fullLine = fullLine.Replace("_goStateActionParamOne_", "Ready");
                                        break;
                                    case 2:
                                        fullLine = fullLine.Replace("_goStateActionParamOne_", "Activated");
                                        break;
                                    case 3:
                                        fullLine = fullLine.Replace("_goStateActionParamOne_", "Deactivated");
                                        break;
                                    default:
                                        fullLine = fullLine.Replace("_goStateActionParamOne_", "<Unknown Gameobject State>");
                                        break;
                                }
                            }

                            if (fullLine.Contains("_getGoFlags_"))
                            {
                                string commentGoFlag = "";
                                int goFlags = smartScript.action_param1;

                                if ((goFlags & (int)GoFlags.GO_FLAG_IN_USE) != 0)           commentGoFlag += "In Use & ";
                                if ((goFlags & (int)GoFlags.GO_FLAG_LOCKED) != 0)           commentGoFlag += "Locked & ";
                                if ((goFlags & (int)GoFlags.GO_FLAG_INTERACT_COND) != 0)    commentGoFlag += "Interact Condition & ";
                                if ((goFlags & (int)GoFlags.GO_FLAG_TRANSPORT) != 0)        commentGoFlag += "Transport & ";
                                if ((goFlags & (int)GoFlags.GO_FLAG_NOT_SELECTABLE) != 0)   commentGoFlag += "Not Selectable & ";
                                if ((goFlags & (int)GoFlags.GO_FLAG_NODESPAWN) != 0)        commentGoFlag += "No Despawn & ";
                                if ((goFlags & (int)GoFlags.GO_FLAG_TRIGGERED) != 0)        commentGoFlag += "Triggered & ";
                                if ((goFlags & (int)GoFlags.GO_FLAG_DAMAGED) != 0)          commentGoFlag += "Damaged & ";
                                if ((goFlags & (int)GoFlags.GO_FLAG_DESTROYED) != 0)        commentGoFlag += "Destroyed & ";

                                commentGoFlag = commentGoFlag.Trim(new[] { ' ', '&', ' ' }); //! Trim last ' & ' from the comment..

                                if (commentGoFlag.Contains("&"))
                                    fullLine = fullLine.Replace("_getGoFlags_", "s_getGoFlags_");

                                fullLine = fullLine.Replace("_getGoFlags_", " " + commentGoFlag);
                            }

                            if (fullLine.Contains("_getDynamicFlags_"))
                            {
                                string commentDynamicFlag = "";
                                int dynamicFlags = smartScript.action_param1;

                                if ((dynamicFlags & (int)DynamicFlags.UNIT_DYNFLAG_NONE) != 0)                      commentDynamicFlag += "None & ";
                                if ((dynamicFlags & (int)DynamicFlags.UNIT_DYNFLAG_LOOTABLE) != 0)                  commentDynamicFlag += "Lootable & ";
                                if ((dynamicFlags & (int)DynamicFlags.UNIT_DYNFLAG_TRACK_UNIT) != 0)                commentDynamicFlag += "Track Units & ";
                                if ((dynamicFlags & (int)DynamicFlags.UNIT_DYNFLAG_TAPPED) != 0)                    commentDynamicFlag += "Tapped & ";
                                if ((dynamicFlags & (int)DynamicFlags.UNIT_DYNFLAG_TAPPED_BY_PLAYER) != 0)          commentDynamicFlag += "Tapped By Player & ";
                                if ((dynamicFlags & (int)DynamicFlags.UNIT_DYNFLAG_SPECIALINFO) != 0)               commentDynamicFlag += "Special Info & ";
                                if ((dynamicFlags & (int)DynamicFlags.UNIT_DYNFLAG_DEAD) != 0)                      commentDynamicFlag += "Dead & ";
                                if ((dynamicFlags & (int)DynamicFlags.UNIT_DYNFLAG_REFER_A_FRIEND) != 0)            commentDynamicFlag += "Refer A Friend & ";
                                if ((dynamicFlags & (int)DynamicFlags.UNIT_DYNFLAG_TAPPED_BY_ALL_THREAT_LIST) != 0) commentDynamicFlag += "Tapped By Threatlist & ";

                                commentDynamicFlag = commentDynamicFlag.Trim(new[] { ' ', '&', ' ' }); //! Trim last ' & ' from the comment..

                                if (commentDynamicFlag.Contains("&"))
                                    fullLine = fullLine.Replace("_getDynamicFlags_", "s_getDynamicFlags_");

                                fullLine = fullLine.Replace("_getDynamicFlags_", " " + commentDynamicFlag);
                            }

                            if (fullLine.Contains("_getBytes1Flags_"))
                            {
                                switch ((UnitFieldBytes1Type)smartScript.action_param2)
                                {
                                    case UnitFieldBytes1Type.UnitStandStateType:
                                    {
                                        switch ((UnitStandStateType)smartScript.action_param1)
                                        {
                                            case UnitStandStateType.UNIT_STAND_STATE_STAND:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Standstate Stand Up");
                                                break;
                                            case UnitStandStateType.UNIT_STAND_STATE_SIT:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Standstate Sit Down");
                                                break;
                                            case UnitStandStateType.UNIT_STAND_STATE_SIT_CHAIR:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Standstate Sit Down Chair");
                                                break;
                                            case UnitStandStateType.UNIT_STAND_STATE_SLEEP:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Standstate Sleep");
                                                break;
                                            case UnitStandStateType.UNIT_STAND_STATE_SIT_LOW_CHAIR:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Standstate Sit Low Chair");
                                                break;
                                            case UnitStandStateType.UNIT_STAND_STATE_SIT_MEDIUM_CHAIR:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Standstate Sit Medium Chair");
                                                break;
                                            case UnitStandStateType.UNIT_STAND_STATE_SIT_HIGH_CHAIR:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Standstate Sit High Chair");
                                                break;
                                            case UnitStandStateType.UNIT_STAND_STATE_DEAD:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Standstate Dead");
                                                break;
                                            case UnitStandStateType.UNIT_STAND_STATE_KNEEL:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Standstate Kneel");
                                                break;
                                            case UnitStandStateType.UNIT_STAND_STATE_SUBMERGED:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Standstate Submerged");
                                                break;
                                            default:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "<Unknown bytes1 (UnitStandStateType)>");
                                                break;
                                        }
                                        break;
                                    }
                                    case UnitFieldBytes1Type.UnitStandFlags:
                                    {
                                        switch ((UnitStandFlags)smartScript.action_param1)
                                        {
                                            case UnitStandFlags.UNIT_STAND_FLAGS_UNK1:
                                            case UnitStandFlags.UNIT_STAND_FLAGS_UNK4:
                                            case UnitStandFlags.UNIT_STAND_FLAGS_UNK5:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "<Unknown>");
                                                break;
                                            case UnitStandFlags.UNIT_STAND_FLAGS_CREEP:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Creep");
                                                break;
                                            case UnitStandFlags.UNIT_STAND_FLAGS_UNTRACKABLE:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Untrackable");
                                                break;
                                            default:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "<Unknown bytes1 (UnitStandFlags)>");
                                                break;
                                        }
                                        break;
                                    }
                                    case UnitFieldBytes1Type.UnitBytes1_Flags:
                                    {
                                        switch ((UnitBytes1_Flags)smartScript.action_param1)
                                        {
                                            case UnitBytes1_Flags.UNIT_BYTE1_FLAG_UNK_3:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "<Unknown>");
                                                break;
                                            case UnitBytes1_Flags.UNIT_BYTE1_FLAG_HOVER:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Hover");
                                                break;
                                            case UnitBytes1_Flags.UNIT_BYTE1_FLAG_ALWAYS_STAND:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "Always Stand");
                                                break;
                                            default:
                                                fullLine = fullLine.Replace("_getBytes1Flags_", "<Unknown bytes1 (UnitBytes1_Flags)>");
                                                break;
                                        }
                                        break;
                                    }
                                }
                            }

                            if (fullLine.Contains("_powerTypeActionParamOne_"))
                            {
                                switch (smartScript.action_param1)
                                {
                                    case 0:
                                        fullLine = fullLine.Replace("_powerTypeActionParamOne_", "Mana");
                                        break;
                                    case 1:
                                        fullLine = fullLine.Replace("_powerTypeActionParamOne_", "Rage");
                                        break;
                                    case 2:
                                        fullLine = fullLine.Replace("_powerTypeActionParamOne_", "Focus");
                                        break;
                                    case 3:
                                        fullLine = fullLine.Replace("_powerTypeActionParamOne_", "Energy");
                                        break;
                                    case 4:
                                        fullLine = fullLine.Replace("_powerTypeActionParamOne_", "Happiness");
                                        break;
                                    case 5:
                                        fullLine = fullLine.Replace("_powerTypeActionParamOne_", "Rune");
                                        break;
                                    case 6:
                                        fullLine = fullLine.Replace("_powerTypeActionParamOne_", "Runic Power");
                                        break;
                                    default:
                                        fullLine = fullLine.Replace("_powerTypeActionParamOne_", "<Unknown Powertype>");
                                        break;
                                }
                            }

                            if (fullLine.Contains("_getUnitFlags_"))
                            {
                                string commentUnitFlag = "";
                                int unitFlags = smartScript.action_param1;

                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_SERVER_CONTROLLED) != 0) commentUnitFlag += "Server Controlled & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_NON_ATTACKABLE) != 0) commentUnitFlag += "Not Attackable & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_DISABLE_MOVE) != 0) commentUnitFlag += "Disable Move & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PVP_ATTACKABLE) != 0) commentUnitFlag += "PvP Attackable & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_RENAME) != 0) commentUnitFlag += "Rename & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PREPARATION) != 0) commentUnitFlag += "Preparation & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_NOT_ATTACKABLE_1) != 0) commentUnitFlag += "Not Attackable & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_IMMUNE_TO_PC) != 0) commentUnitFlag += "Immune To Players & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_IMMUNE_TO_NPC) != 0) commentUnitFlag += "Immune To Creatures & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_LOOTING) != 0) commentUnitFlag += "Looting & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PET_IN_COMBAT) != 0) commentUnitFlag += "Pet In Combat & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PVP) != 0) commentUnitFlag += "PvP & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_SILENCED) != 0) commentUnitFlag += "Silenced & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PACIFIED) != 0) commentUnitFlag += "Pacified & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_STUNNED) != 0) commentUnitFlag += "Stunned & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_IN_COMBAT) != 0) commentUnitFlag += "In Combat & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_TAXI_FLIGHT) != 0) commentUnitFlag += "Taxi Flight & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_DISARMED) != 0) commentUnitFlag += "Disarmed & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_CONFUSED) != 0) commentUnitFlag += "Confused & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_FLEEING) != 0) commentUnitFlag += "Fleeing & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_PLAYER_CONTROLLED) != 0) commentUnitFlag += "Player Controlled & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_NOT_SELECTABLE) != 0) commentUnitFlag += "Not Selectable & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_SKINNABLE) != 0) commentUnitFlag += "Skinnable & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_MOUNT) != 0) commentUnitFlag += "Mounted & ";
                                if ((unitFlags & (int)UnitFlags.UNIT_FLAG_SHEATHE) != 0) commentUnitFlag += "Sheathed & ";

                                commentUnitFlag = commentUnitFlag.Trim(new[] { ' ', '&', ' ' }); //! Trim last ' & ' from the comment..

                                if (commentUnitFlag.Contains("&"))
                                    fullLine = fullLine.Replace("_getUnitFlags_", "s_getUnitFlags_");

                                fullLine = fullLine.Replace("_getUnitFlags_", " " + commentUnitFlag);
                            }

                            if (smartScript.event_flags > 0)
                            {
                                if ((((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_NOT_REPEATABLE) != 0))
                                    fullLine += " (No Repeat)";

                                if ((((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_0) != 0) && (((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_1) != 0) &&
                                    (((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_2) != 0) && (((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_3) != 0))
                                    fullLine += " (Dungeon & Raid)";
                                else
                                {
                                    if ((((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_0) != 0) && (((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_1) != 0))
                                        fullLine += " (Dungeon)";
                                    else
                                    {
                                        if ((((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_0) != 0))
                                            fullLine += " (Normal Dungeon)";
                                        else if ((((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_1) != 0))
                                            fullLine += " (Heroic Dungeon)";
                                    }

                                    if ((((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_2) != 0) && (((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_3) != 0))
                                        fullLine += " (Raid)";
                                    else
                                    {
                                        if ((((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_2) != 0))
                                            fullLine += " (Normal Raid)";
                                        else if ((((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DIFFICULTY_3) != 0))
                                            fullLine += " (Heroic Raid)";
                                    }
                                }

                                if ((((SmartEventFlags)smartScript.event_flags & SmartEventFlags.SMART_EVENT_FLAG_DEBUG_ONLY) != 0))
                                    fullLine += " (Debug)";
                            }

                            if (smartScript.event_phase_mask > 0)
                            {
                                List<int> listOfSplitPhases = new List<int>();

                                int event_phase_mask = smartScript.event_phase_mask;
                                int event_phase_mask2 = event_phase_mask;
                                int log2 = 0;

                                while (event_phase_mask2 >= 2)
                                {
                                    event_phase_mask2 /= 2;
                                    log2++;
                                }

                                for (int l2 = log2; l2 >= 0; l2--)
                                {
                                    int power = (int)Math.Pow(2, l2);

                                    if (event_phase_mask >= power)
                                    {
                                        event_phase_mask -= power;
                                        listOfSplitPhases.Add(power);
                                    }
                                }

                                int[] arrayOfSplitPhases = listOfSplitPhases.ToArray();
                                Array.Reverse(arrayOfSplitPhases); //! Reverse them so they are ascending
                                fullLine += " (Phase";

                                if (listOfSplitPhases.Count > 1)
                                    fullLine += "s";

                                fullLine += " " + String.Join(" & ", arrayOfSplitPhases) + ")";
                            }

                            string cleanNewComment = fullLine;
                            fullLine = fullLine.Insert(0, "UPDATE `smart_scripts` SET `comment`=" + '"');

                            //! Don't update the script if the comment is already correct
                            if (cleanNewComment == smartScript.comment)
                            {
                                totalSkippedScripts++;
                                continue;
                            }

                            fullLine += '"' + " WHERE `source_type`=" + smartScript.source_type + " AND `entryorguid`=" + smartScript.entryorguid + " AND `id`=" + smartScript.id + ';';
                            Console.WriteLine(fullLine);
                            fullLine += " -- Old comment: '" + smartScript.comment + "'"; //! Don't print the old comment in console
                            outputFile.WriteLine(fullLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n\n" + ex.Message);
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
            }
            finally
            {
                Console.WriteLine("\n\n\nThe converting has finished. A total of {0} scripts were loaded of which {1} were skipped because their comments already fit the correct codestyle.", totalLoadedScripts, totalSkippedScripts);
                Console.WriteLine("If you wish to open the output file with your selected .sql file editor, press Enter.");

                if (Console.ReadKey().Key == ConsoleKey.Enter)
                    Process.Start("output.sql");
            }
        }

        private static SmartScript BuildSmartScript(DataRow row)
        {
            var smartScript = new SmartScript();
            smartScript.entryorguid = row["entryorguid"] != DBNull.Value ? Convert.ToInt32(row["entryorguid"]) : -1;
            smartScript.source_type = row["source_type"] != DBNull.Value ? Convert.ToInt32(row["source_type"]) : 0;
            smartScript.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
            smartScript.link = row["link"] != DBNull.Value ? Convert.ToInt32(row["link"]) : 0;
            smartScript.event_type = row["event_type"] != DBNull.Value ? Convert.ToInt32(row["event_type"]) : 0;
            smartScript.event_phase_mask = row["event_phase_mask"] != DBNull.Value ? Convert.ToInt32(row["event_phase_mask"]) : 0;
            smartScript.event_chance = row["event_chance"] != DBNull.Value ? Convert.ToInt32(row["event_chance"]) : 0;
            smartScript.event_flags = row["event_flags"] != DBNull.Value ? Convert.ToInt32(row["event_flags"]) : 0;
            smartScript.event_param1 = row["event_param1"] != DBNull.Value ? Convert.ToInt32(row["event_param1"]) : 0;
            smartScript.event_param2 = row["event_param2"] != DBNull.Value ? Convert.ToInt32(row["event_param2"]) : 0;
            smartScript.event_param3 = row["event_param3"] != DBNull.Value ? Convert.ToInt32(row["event_param3"]) : 0;
            smartScript.event_param4 = row["event_param4"] != DBNull.Value ? Convert.ToInt32(row["event_param4"]) : 0;
            smartScript.action_type = row["action_type"] != DBNull.Value ? Convert.ToInt32(row["action_type"]) : 0;
            smartScript.action_param1 = row["action_param1"] != DBNull.Value ? Convert.ToInt32(row["action_param1"]) : 0;
            smartScript.action_param2 = row["action_param2"] != DBNull.Value ? Convert.ToInt32(row["action_param2"]) : 0;
            smartScript.action_param3 = row["action_param3"] != DBNull.Value ? Convert.ToInt32(row["action_param3"]) : 0;
            smartScript.action_param4 = row["action_param4"] != DBNull.Value ? Convert.ToInt32(row["action_param4"]) : 0;
            smartScript.action_param5 = row["action_param5"] != DBNull.Value ? Convert.ToInt32(row["action_param5"]) : 0;
            smartScript.action_param6 = row["action_param6"] != DBNull.Value ? Convert.ToInt32(row["action_param6"]) : 0;
            smartScript.target_type = row["target_type"] != DBNull.Value ? Convert.ToInt32(row["target_type"]) : 0;
            smartScript.target_param1 = row["target_param1"] != DBNull.Value ? Convert.ToInt32(row["target_param1"]) : 0;
            smartScript.target_param2 = row["target_param2"] != DBNull.Value ? Convert.ToInt32(row["target_param2"]) : 0;
            smartScript.target_param3 = row["target_param3"] != DBNull.Value ? Convert.ToInt32(row["target_param3"]) : 0;
            smartScript.target_x = row["target_x"] != DBNull.Value ? Convert.ToInt32(row["target_x"]) : 0;
            smartScript.target_y = row["target_y"] != DBNull.Value ? Convert.ToInt32(row["target_y"]) : 0;
            smartScript.target_z = row["target_z"] != DBNull.Value ? Convert.ToInt32(row["target_z"]) : 0;
            smartScript.target_o = row["target_o"] != DBNull.Value ? Convert.ToInt32(row["target_o"]) : 0;
            smartScript.comment = row["comment"] != DBNull.Value ? (string)row["comment"] : String.Empty;
            return smartScript;
        }

        private static string GetCreatureNameByEntry(MySqlConnection connection, int entry)
        {
            MySqlCommand command = new MySqlCommand(String.Format("SELECT name FROM creature_template WHERE entry={0}", entry), connection);
            MySqlDataReader readerSourceName = command.ExecuteReader(CommandBehavior.Default);

            if (readerSourceName.Read())
            {
                string str = readerSourceName[0].ToString();
                readerSourceName.Close();
                return str;
            }

            readerSourceName.Close();
            return String.Empty;
        }

        private static int GetCreatureIdByGuid(MySqlConnection connection, int guid)
        {
            MySqlCommand command = new MySqlCommand(String.Format("SELECT id FROM creature WHERE guid={0}", guid), connection);
            MySqlDataReader readerSourceEntry = command.ExecuteReader(CommandBehavior.Default);

            if (readerSourceEntry.Read())
            {
                int entry = Convert.ToInt32(readerSourceEntry[0]);
                readerSourceEntry.Close();
                return entry;
            }

            readerSourceEntry.Close();
            return -1;
        }

        private static string GetCreatureNameByGuid(MySqlConnection connection, int guid)
        {
            MySqlCommand command = new MySqlCommand(String.Format("SELECT name FROM creature_template WHERE entry={0}", GetCreatureIdByGuid(connection, guid)), connection);
            MySqlDataReader readerSourceName = command.ExecuteReader(CommandBehavior.Default);

            if (readerSourceName.Read())
            {
                string str = readerSourceName[0].ToString();
                readerSourceName.Close();
                return str;
            }

            readerSourceName.Close();
            return String.Empty;
        }

        private static string GetGameobjectNameByEntry(MySqlConnection connection, int entry)
        {
            MySqlCommand command = new MySqlCommand(String.Format("SELECT name FROM gameobject_template WHERE entry={0}", entry), connection);
            MySqlDataReader readerSourceName = command.ExecuteReader(CommandBehavior.Default);

            if (readerSourceName.Read())
            {
                string str = readerSourceName[0].ToString();
                readerSourceName.Close();
                return str;
            }

            readerSourceName.Close();
            return String.Empty;
        }

        private static int GetGameobjectIdByGuid(MySqlConnection connection, int guid)
        {
            MySqlCommand command = new MySqlCommand(String.Format("SELECT id FROM gameobject WHERE guid={0}", guid), connection);
            MySqlDataReader readerSourceEntry = command.ExecuteReader(CommandBehavior.Default);

            if (readerSourceEntry.Read())
            {
                int entry = Convert.ToInt32(readerSourceEntry[0]);
                readerSourceEntry.Close();
                return entry;
            }

            readerSourceEntry.Close();
            return -1;
        }

        private static string GetGameobjectNameByGuid(MySqlConnection connection, int guid)
        {
            MySqlCommand command = new MySqlCommand(String.Format("SELECT name FROM gameobject_template WHERE entry={0}", GetGameobjectIdByGuid(connection, guid)), connection);
            MySqlDataReader readerSourceName = command.ExecuteReader(CommandBehavior.Default);

            if (readerSourceName.Read())
            {
                string str = readerSourceName[0].ToString();
                readerSourceName.Close();
                return str;
            }

            readerSourceName.Close();
            return String.Empty;
        }

        private static string GetStringByTargetType(SmartScript smartScript, MySqlConnection connection)
        {
            switch ((SmartTargetType)smartScript.target_type)
            {
                case SmartTargetType.SMART_TARGET_VICTIM:
                    return "Victim";
                case SmartTargetType.SMART_TARGET_HOSTILE_SECOND_AGGRO:
                    return "Second On Threatlist";
                case SmartTargetType.SMART_TARGET_HOSTILE_LAST_AGGRO:
                    return "Last On Threatlist";
                case SmartTargetType.SMART_TARGET_HOSTILE_RANDOM:
                    return "Random On Threatlist";
                case SmartTargetType.SMART_TARGET_HOSTILE_RANDOM_NOT_TOP:
                    return "Random On Threatlist Not Top";
                case SmartTargetType.SMART_TARGET_ACTION_INVOKER:
                    return "Invoker";
                case SmartTargetType.SMART_TARGET_POSITION:
                    return "Position";
                case SmartTargetType.SMART_TARGET_CREATURE_RANGE:
                case SmartTargetType.SMART_TARGET_CREATURE_DISTANCE:
                case SmartTargetType.SMART_TARGET_CLOSEST_CREATURE:
                    return "Closest Creature '" + GetCreatureNameByEntry(connection, smartScript.target_param1) + "'";
                case SmartTargetType.SMART_TARGET_CREATURE_GUID:
                    return "Closest Creature '" + GetCreatureNameByGuid(connection, smartScript.target_param1) + "'";
                case SmartTargetType.SMART_TARGET_GAMEOBJECT_RANGE:
                case SmartTargetType.SMART_TARGET_GAMEOBJECT_DISTANCE:
                case SmartTargetType.SMART_TARGET_CLOSEST_GAMEOBJECT:
                    return "Closest Gameobject '" + GetGameobjectNameByEntry(connection, smartScript.target_param1) + "'";
                case SmartTargetType.SMART_TARGET_GAMEOBJECT_GUID:
                    return "Closest Gameobject '" + GetGameobjectNameByGuid(connection, smartScript.target_param1) + "'";
                case SmartTargetType.SMART_TARGET_INVOKER_PARTY:
                    return "Invoker's Party";
                case SmartTargetType.SMART_TARGET_PLAYER_RANGE:
                case SmartTargetType.SMART_TARGET_PLAYER_DISTANCE:
                case SmartTargetType.SMART_TARGET_CLOSEST_PLAYER:
                    return "Closest Player";
                case SmartTargetType.SMART_TARGET_ACTION_INVOKER_VEHICLE:
                    return "Invoker's Vehicle";
                case SmartTargetType.SMART_TARGET_OWNER_OR_SUMMONER:
                    return "Owner Or Summoner";
                case SmartTargetType.SMART_TARGET_THREAT_LIST:
                    return "First Unit On Threatlist";
                case SmartTargetType.SMART_TARGET_CLOSEST_ENEMY:
                    return "Closest Enemy";
                case SmartTargetType.SMART_TARGET_CLOSEST_FRIENDLY:
                    return "Closest Friendly Unit";
                default:
                    return "<unsupported target type>";
            }
        }
    }
}
