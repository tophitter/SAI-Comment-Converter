﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;

namespace SAI_Comment_Converter
{
    class Program
    {
        private static Dictionary<SmartEvent, string> smartEventStrings = new Dictionary<SmartEvent, string>();
        private static Dictionary<SmartAction, string> smartActionStrings = new Dictionary<SmartAction, string>();

        static void Main(string[] args)
        {
            smartEventStrings.Add(SmartEvent.SMART_EVENT_SPELLHIT, "On Spellhit '_spellNameEventParamOne_'");
            smartEventStrings.Add(SmartEvent.SMART_EVENT_HAS_AURA, "On Has Aura '_spellNameEventParamOne_'");
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
            smartEventStrings.Add(SmartEvent.SMART_EVENT_TEXT_OVER, "On Text _eventParamOne_ Finished");
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
            smartEventStrings.Add(SmartEvent.SMART_EVENT_TARGET_CASTING, "Target Casting _targetCastingSpellName_");
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
            smartActionStrings.Add(SmartAction.SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL, "todo dbc"); // todo
            smartActionStrings.Add(SmartAction.SMART_ACTION_SOUND, "Play Sound _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_EMOTE, "Play Emote _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_FAIL_QUEST, "Fail Quest '_questNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_QUEST, "Add Quest '_questNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_REACT_STATE, "Set Reactstate _reactStateParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ACTIVATE_GOBJECT, "Activate Gameobject");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RANDOM_EMOTE, "Play Random Emote (_actionRandomEmotes_)"); // todo
            smartActionStrings.Add(SmartAction.SMART_ACTION_CAST, "Cast '_spellNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SUMMON_CREATURE, "Summon Creature '_creatureNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_THREAT_SINGLE_PCT, "Set Single Threat _actionParamOne_-_actionParamTwo_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_THREAT_ALL_PCT, "Set All Threat _actionParamOne_-_actionParamTwo_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS, "Give Quest Credit '_questNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_UNUSED_16, "Unused Action Type (16)");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_EMOTE_STATE, "Set Emote State _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_UNIT_FLAG, "Set Flag_getUnitFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_UNIT_FLAG, "Remove Flag_getUnitFlags_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_AUTO_ATTACK, "_startOrStopActionParamOne_ Attacking");
            smartActionStrings.Add(SmartAction.SMART_ACTION_COMBAT_MOVEMENT, "_startOrStopActionParamOne_ Combat Movement");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_EVENT_PHASE, "Set Event Phase _actionParamOne_");
            smartActionStrings.Add(SmartAction.SMART_ACTION_INC_EVENT_PHASE, "_incrementOrDecrementActionParamOne_ Phase");
            smartActionStrings.Add(SmartAction.SMART_ACTION_EVADE, "Evade");
            smartActionStrings.Add(SmartAction.SMART_ACTION_FLEE_FOR_ASSIST, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_GROUPEVENTHAPPENS, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_CASTEDCREATUREORGO, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVEAURASFROMSPELL, "Remove Aura '_spellNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_FOLLOW, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RANDOM_PHASE, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RANDOM_PHASE_RANGE, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RESET_GOBJECT, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_KILLED_MONSTER, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_INST_DATA, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_INST_DATA64, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_UPDATE_TEMPLATE, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_DIE, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_IN_COMBAT_WITH_ZONE, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_FOR_HELP, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_SHEATH, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_FORCE_DESPAWN, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_INVINCIBILITY_HP_LEVEL, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_PHASE_MASK, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_DATA, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_MOVE_FORWARD, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_VISIBILITY, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_ACTIVE, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ATTACK_START, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SUMMON_GO, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_KILL_UNIT, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ACTIVATE_TAXI, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_WP_START, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_WP_PAUSE, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_WP_STOP, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_ITEM, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_ITEM, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_INSTALL_AI_TEMPLATE, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_RUN, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_FLY, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_SWIM, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_TELEPORT, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_STORE_VARIABLE_DECIMAL, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_STORE_TARGET_LIST, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_WP_RESUME, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_ORIENTATION, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CREATE_TIMED_EVENT, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_PLAYMOVIE, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_MOVE_TO_POS, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RESPAWN_TARGET, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_EQUIP, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CLOSE_GOSSIP, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_TRIGGER_TIMED_EVENT, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_TIMED_EVENT, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_AURA, "Add Aura '_spellNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_OVERRIDE_SCRIPT_BASE_OBJECT, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RESET_SCRIPT_BASE_OBJECT, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_SCRIPT_RESET, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_RANGED_MOVEMENT, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_TIMED_ACTIONLIST, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_NPC_FLAG, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_NPC_FLAG, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_NPC_FLAG, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SIMPLE_TALK, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_INVOKER_CAST, "Invoker Cast '_spellNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CROSS_CAST, "Cross Cast '_spellNameActionParamOne_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_RANDOM_TIMED_ACTIONLIST, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_RANDOM_MOVE, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_UNIT_FIELD_BYTES_1, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_INTERRUPT_SPELL, "Interrupt Spell '_spellNameActionParamTwo_'");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SEND_GO_CUSTOM_ANIM, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_DYNAMIC_FLAG, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_DYNAMIC_FLAG, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_DYNAMIC_FLAG, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_JUMP_TO_POS, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SEND_GOSSIP_MENU, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_GO_SET_LOOT_STATE, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SEND_TARGET_TO_TARGET, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_HOME_POS, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_HEALTH_REGEN, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_ROOT, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_GO_FLAG, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_GO_FLAG, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_GO_FLAG, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SUMMON_CREATURE_GROUP, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_SET_POWER, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_ADD_POWER, "");
            smartActionStrings.Add(SmartAction.SMART_ACTION_REMOVE_POWER, "");

            string host = "127.0.0.1";
            string user = "root";
            string pass = "1234";
            string worldDB = "trinitycore_world";
            string port = "3306";

            while (true)
            {
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

                //string host = "127.0.0.1";
                //string user = "root";
                //string pass = "1234";
                //string worldDB = "trinitycore_world";
                //string port = "3306";

                //Console.WriteLine(host);
                //Console.WriteLine(user);
                //Console.WriteLine(pass);
                //Console.WriteLine(worldDB);
                //Console.WriteLine(port);

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
                        break;

                    File.Delete("output.sql");

                    using (var outputFile = new StreamWriter("output.sql", true))
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            MySqlCommand command = new MySqlCommand();
                            command.Connection = connection;

                            string fullLine = "UPDATE `smart_scripts` SET `comment`='";
                            int entryorguid = Convert.ToInt32(row.ItemArray[0].ToString());
                            int entry = entryorguid;
                            MySqlDataReader readerSourceName = null;
                            MySqlDataReader readerSourceId = null;

                            //! Sourcetype switch
                            switch (Convert.ToInt32(row.ItemArray[1].ToString()))
                            {
                                case 0: //! Creature
                                    if (Convert.ToInt32(row.ItemArray[0].ToString()) < 0)
                                    {
                                        command.CommandText = (String.Format("SELECT id FROM creature WHERE guid={0}", -entryorguid));
                                        readerSourceId = command.ExecuteReader(CommandBehavior.Default);

                                        if (readerSourceId.Read())
                                            entry = Convert.ToInt32(readerSourceId[0].ToString());

                                        readerSourceId.Close();
                                    }

                                    command.CommandText = (String.Format("SELECT name FROM creature_template WHERE entry={0}", entry));
                                    readerSourceName = command.ExecuteReader(CommandBehavior.Default);

                                    if (readerSourceName.Read())
                                        fullLine += readerSourceName[0].ToString() + " - ";

                                    readerSourceName.Close();
                                    break;
                                case 1: //! Gammeobject
                                    if (Convert.ToInt32(row.ItemArray[0].ToString()) < 0)
                                    {
                                        command.CommandText = (String.Format("SELECT id FROM gameobject WHERE guid={0}", -entryorguid));
                                        readerSourceId = command.ExecuteReader(CommandBehavior.Default);

                                        if (readerSourceId.Read())
                                            entry = Convert.ToInt32(readerSourceId[0].ToString());

                                        readerSourceId.Close();
                                    }

                                    command.CommandText = (String.Format("SELECT name FROM gameobject_template WHERE entry={0}", entry));
                                    readerSourceName = command.ExecuteReader(CommandBehavior.Default);

                                    if (readerSourceName.Read())
                                        fullLine += readerSourceName[0].ToString() + " - ";

                                    readerSourceName.Close();
                                    break;
                                case 2: //! Areatrigger
                                    continue;
                                case 9: //! Actionlist
                                    continue;
                            }

                            //! Event type
                            fullLine += smartEventStrings[(SmartEvent)Convert.ToInt32(row.ItemArray[4].ToString())];

                            //! TODO: Figure out how to make this work with linking several lines TO each other. Perhaps read from last to first line?
                            //! TODO: Consider linkto/linkfrom
                            // SELECT * FROM smart_scripts ORDER BY entryorguid ASC, id DESC
                            if (fullLine.Contains("_previousLineComment_"))
                            {
                                MySqlCommand commandPreviousComment = new MySqlCommand(String.Format("SELECT event_type FROM smart_scripts WHERE entryorguid={0} AND id={1}", entryorguid, (Convert.ToInt32(row.ItemArray[2]) - 1).ToString()), connection);
                                MySqlDataReader readerPreviousLineComment = commandPreviousComment.ExecuteReader(CommandBehavior.Default);

                                if (readerPreviousLineComment.Read())
                                    fullLine = fullLine.Replace("_previousLineComment_", smartEventStrings[(SmartEvent)Convert.ToInt32(readerPreviousLineComment[0].ToString())]);
                                else
                                    fullLine = fullLine.Replace("_previousLineComment_", "Link not found!");

                                readerPreviousLineComment.Close();
                            }

                            //! This must be called AFTER we check for _previousLineComment_ so that copied event types don't need special handling
                            if (fullLine.Contains("_eventParamOne_"))
                                fullLine = fullLine.Replace("_eventParamOne_", row.ItemArray[8].ToString());

                            if (fullLine.Contains("_eventParamTwo_"))
                                fullLine = fullLine.Replace("_eventParamTwo_", row.ItemArray[9].ToString());

                            if (fullLine.Contains("_eventParamThree_"))
                                fullLine = fullLine.Replace("_eventParamThree_", row.ItemArray[10].ToString());

                            if (fullLine.Contains("_eventParamFour_"))
                                fullLine = fullLine.Replace("_eventParamFour_", row.ItemArray[11].ToString());

                            if (fullLine.Contains("_spellNameEventParamOne_"))
                            {
                                MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT spellName FROM spells_dbc WHERE id = {0}", row.ItemArray[8].ToString()), connection);
                                MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                if (readerSelect.Read())
                                    fullLine = fullLine.Replace("_spellNameEventParamOne_", readerSelect[0].ToString());
                                else
                                    fullLine = fullLine.Replace("_spellNameEventParamOne_", "Spell not found!");

                                readerSelect.Close();
                            }

                            if (fullLine.Contains("_targetCastingSpellName_"))
                            {
                                if (row.ItemArray[10].ToString() != "0")
                                {
                                    MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT spellName FROM spells_dbc WHERE id = {0}", row.ItemArray[10].ToString()), connection);
                                    MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                    if (readerSelect.Read())
                                        fullLine = fullLine.Replace("_targetCastingSpellName_", "'" + readerSelect[0].ToString() + "'");
                                    else
                                        fullLine = fullLine.Replace("_targetCastingSpellName_", "Spell not found!");

                                    readerSelect.Close();
                                }
                                else
                                    fullLine = fullLine.Replace(" _targetCastingSpellName_", String.Empty);
                            }

                            if (fullLine.Contains("_questNameEventParamOne_"))
                            {
                                if (row.ItemArray[8].ToString() == "0") //! Any quest (SMART_EVENT_ACCEPTED_QUEST / SMART_EVENT_REWARD_QUEST)
                                    fullLine = fullLine.Replace(" '_questNameEventParamOne_'", String.Empty);
                                else
                                {
                                    MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT title FROM quest_template WHERE id = {0}", row.ItemArray[8].ToString()), connection);
                                    MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                    if (readerSelect.Read())
                                        fullLine = fullLine.Replace("_questNameEventParamOne_", readerSelect[0].ToString());
                                    else
                                        fullLine = fullLine.Replace("_questNameEventParamOne_", " Quest not found!");

                                    readerSelect.Close();
                                }
                            }

                            //! Action type
                            fullLine += " - " + smartActionStrings[(SmartAction)Convert.ToInt32(row.ItemArray[12].ToString())];

                            if (fullLine.Contains("_actionParamOne_"))
                                fullLine = fullLine.Replace("_actionParamOne_", row.ItemArray[13].ToString());

                            if (fullLine.Contains("_actionParamTwo_"))
                                fullLine = fullLine.Replace("_actionParamTwo_", row.ItemArray[14].ToString());

                            if (fullLine.Contains("_actionParamThree_"))
                                fullLine = fullLine.Replace("_actionParamThree_", row.ItemArray[15].ToString());

                            if (fullLine.Contains("_actionParamFour_"))
                                fullLine = fullLine.Replace("_actionParamFour_", row.ItemArray[16].ToString());

                            if (fullLine.Contains("_actionParamFive_"))
                                fullLine = fullLine.Replace("_actionParamFive_", row.ItemArray[17].ToString());

                            if (fullLine.Contains("_actionParamSix_"))
                                fullLine = fullLine.Replace("_actionParamSix_", row.ItemArray[18].ToString());

                            if (fullLine.Contains("_spellNameActionParamOne_"))
                            {
                                MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT spellName FROM spells_dbc WHERE id = {0}", row.ItemArray[13].ToString()), connection);
                                MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                if (readerSelect.Read())
                                    fullLine = fullLine.Replace("_spellNameActionParamOne_", readerSelect[0].ToString());
                                else
                                    fullLine = fullLine.Replace("_spellNameActionParamOne_", "Spell not found!");

                                readerSelect.Close();
                            }

                            if (fullLine.Contains("_spellNameActionParamTwo_"))
                            {
                                MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT spellName FROM spells_dbc WHERE id = {0}", row.ItemArray[14].ToString()), connection);
                                MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                if (readerSelect.Read())
                                    fullLine = fullLine.Replace("_spellNameActionParamTwo_", readerSelect[0].ToString());
                                else
                                    fullLine = fullLine.Replace("_spellNameActionParamTwo_", "Spell not found!");

                                readerSelect.Close();
                            }

                            if (fullLine.Contains("_questNameActionParamOne_"))
                            {
                                MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT title FROM quest_template WHERE id = {0}", row.ItemArray[13].ToString()), connection);
                                MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                if (readerSelect.Read())
                                    fullLine = fullLine.Replace("_questNameActionParamOne_", readerSelect[0].ToString());
                                else
                                    fullLine = fullLine.Replace("_questNameActionParamOne_", "Quest not found!");

                                readerSelect.Close();
                            }

                            if (fullLine.Contains("_reactStateParamOne_"))
                            {
                                switch (row.ItemArray[13].ToString())
                                {
                                    case "0":
                                        fullLine = fullLine.Replace("_reactStateParamOne_", "Passive");
                                        break;
                                    case "1":
                                        fullLine = fullLine.Replace("_reactStateParamOne_", "Defensive");
                                        break;
                                    case "2":
                                        fullLine = fullLine.Replace("_reactStateParamOne_", "Aggressive");
                                        break;
                                    default:
                                        fullLine = fullLine.Replace("_reactStateParamOne_", "<Unknown Reactstate>");
                                        break;
                                }
                            }

                            if (fullLine.Contains("_creatureNameActionParamOne_"))
                            {
                                MySqlCommand commandSelect = new MySqlCommand(String.Format("SELECT name FROM creature_template WHERE entry = {0}", row.ItemArray[13].ToString()), connection);
                                MySqlDataReader readerSelect = commandSelect.ExecuteReader(CommandBehavior.Default);

                                if (readerSelect.Read())
                                    fullLine = fullLine.Replace("_creatureNameActionParamOne_", readerSelect[0].ToString());
                                else
                                    fullLine = fullLine.Replace("_creatureNameActionParamOne_", "Creature not found!");

                                readerSelect.Close();
                            }

                            if (fullLine.Contains("_getUnitFlags_"))
                            {
                                string commentUnitFlag = "";
                                int unitFlags = Convert.ToInt32(row.ItemArray[13].ToString());

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

                                commentUnitFlag = commentUnitFlag.Trim(new Char[] { ' ', '&', ' ' }); //! Trim last ' & ' from the comment..

                                if (commentUnitFlag.Contains("&"))
                                    fullLine = fullLine.Replace("_getUnitFlags_", "s_getUnitFlags_");

                                fullLine = fullLine.Replace("_getUnitFlags_", " " + commentUnitFlag);
                            }

                            if (fullLine.Contains("_startOrStopActionParamOne_"))
                            {
                                if (row.ItemArray[13].ToString() == "0")
                                    fullLine = fullLine.Replace("_startOrStopActionParamOne_", "Stop");
                                else //! Even if above 1 or below 0 we start attacking/allow-combat-movement
                                    fullLine = fullLine.Replace("_startOrStopActionParamOne_", "Start");
                            }

                            if (fullLine.Contains("_incrementOrDecrementActionParamOne_"))
                            {
                                if (row.ItemArray[13].ToString() == "1")
                                    fullLine = fullLine.Replace("_incrementOrDecrementActionParamOne_", "Increment");
                                else if (row.ItemArray[14].ToString() == "1")
                                    fullLine = fullLine.Replace("_incrementOrDecrementActionParamOne_", "Decrement");
                                //else //? What to do?
                            }

                            string cleanNewComment = fullLine.Replace("UPDATE `smart_scripts` SET `comment`='", String.Empty);

                            //! Don't update the script if the comment is already correct
                            if (cleanNewComment == row.ItemArray[27].ToString())
                                continue;

                            Console.WriteLine(fullLine);
                            outputFile.WriteLine(fullLine);
                        }
                    }
                }
            }
        }
    }
}
