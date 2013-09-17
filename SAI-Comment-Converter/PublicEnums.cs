﻿namespace SAI_Comment_Converter
{
    public enum SmartAction
    {
        SMART_ACTION_NONE = 0,
        SMART_ACTION_TALK = 1,
        SMART_ACTION_SET_FACTION = 2,
        SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL = 3,
        SMART_ACTION_SOUND = 4,
        SMART_ACTION_EMOTE = 5,
        SMART_ACTION_FAIL_QUEST = 6,
        SMART_ACTION_ADD_QUEST = 7,
        SMART_ACTION_SET_REACT_STATE = 8,
        SMART_ACTION_ACTIVATE_GOBJECT = 9,
        SMART_ACTION_RANDOM_EMOTE = 10,
        SMART_ACTION_CAST = 11,
        SMART_ACTION_SUMMON_CREATURE = 12,
        SMART_ACTION_THREAT_SINGLE_PCT = 13,
        SMART_ACTION_THREAT_ALL_PCT = 14,
        SMART_ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS = 15,
        SMART_ACTION_SEND_CASTCREATUREORGO = 16,
        SMART_ACTION_SET_EMOTE_STATE = 17,
        SMART_ACTION_SET_UNIT_FLAG = 18,
        SMART_ACTION_REMOVE_UNIT_FLAG = 19,
        SMART_ACTION_AUTO_ATTACK = 20,
        SMART_ACTION_COMBAT_MOVEMENT = 21,
        SMART_ACTION_SET_EVENT_PHASE = 22,
        SMART_ACTION_INC_EVENT_PHASE = 23,
        SMART_ACTION_EVADE = 24,
        SMART_ACTION_FLEE_FOR_ASSIST = 25,
        SMART_ACTION_CALL_GROUPEVENTHAPPENS = 26,
        SMART_ACTION_CALL_CASTEDCREATUREORGO = 27,
        SMART_ACTION_REMOVEAURASFROMSPELL = 28,
        SMART_ACTION_FOLLOW = 29,
        SMART_ACTION_RANDOM_PHASE = 30,
        SMART_ACTION_RANDOM_PHASE_RANGE = 31,
        SMART_ACTION_RESET_GOBJECT = 32,
        SMART_ACTION_KILLED_MONSTER = 33,
        SMART_ACTION_SET_INST_DATA = 34,
        SMART_ACTION_SET_INST_DATA64 = 35,
        SMART_ACTION_UPDATE_TEMPLATE = 36,
        SMART_ACTION_DIE = 37,
        SMART_ACTION_SET_IN_COMBAT_WITH_ZONE = 38,
        SMART_ACTION_CALL_FOR_HELP = 39,
        SMART_ACTION_SET_SHEATH = 40,
        SMART_ACTION_FORCE_DESPAWN = 41,
        SMART_ACTION_SET_INVINCIBILITY_HP_LEVEL = 42,
        SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL = 43,
        SMART_ACTION_SET_PHASE_MASK = 44,
        SMART_ACTION_SET_DATA = 45,
        SMART_ACTION_MOVE_FORWARD = 46,
        SMART_ACTION_SET_VISIBILITY = 47,
        SMART_ACTION_SET_ACTIVE = 48,
        SMART_ACTION_ATTACK_START = 49,
        SMART_ACTION_SUMMON_GO = 50,
        SMART_ACTION_KILL_UNIT = 51,
        SMART_ACTION_ACTIVATE_TAXI = 52,
        SMART_ACTION_WP_START = 53,
        SMART_ACTION_WP_PAUSE = 54,
        SMART_ACTION_WP_STOP = 55,
        SMART_ACTION_ADD_ITEM = 56,
        SMART_ACTION_REMOVE_ITEM = 57,
        SMART_ACTION_INSTALL_AI_TEMPLATE = 58,
        SMART_ACTION_SET_RUN = 59,
        SMART_ACTION_SET_FLY = 60,
        SMART_ACTION_SET_SWIM = 61,
        SMART_ACTION_TELEPORT = 62,
        SMART_ACTION_STORE_VARIABLE_DECIMAL = 63,
        SMART_ACTION_STORE_TARGET_LIST = 64,
        SMART_ACTION_WP_RESUME = 65,
        SMART_ACTION_SET_ORIENTATION = 66,
        SMART_ACTION_CREATE_TIMED_EVENT = 67,
        SMART_ACTION_PLAYMOVIE = 68,
        SMART_ACTION_MOVE_TO_POS = 69,
        SMART_ACTION_RESPAWN_TARGET = 70,
        SMART_ACTION_EQUIP = 71,
        SMART_ACTION_CLOSE_GOSSIP = 72,
        SMART_ACTION_TRIGGER_TIMED_EVENT = 73,
        SMART_ACTION_REMOVE_TIMED_EVENT = 74,
        SMART_ACTION_ADD_AURA = 75,
        SMART_ACTION_OVERRIDE_SCRIPT_BASE_OBJECT = 76,
        SMART_ACTION_RESET_SCRIPT_BASE_OBJECT = 77,
        SMART_ACTION_CALL_SCRIPT_RESET = 78,
        SMART_ACTION_SET_RANGED_MOVEMENT = 79,
        SMART_ACTION_CALL_TIMED_ACTIONLIST = 80,
        SMART_ACTION_SET_NPC_FLAG = 81,
        SMART_ACTION_ADD_NPC_FLAG = 82,
        SMART_ACTION_REMOVE_NPC_FLAG = 83,
        SMART_ACTION_SIMPLE_TALK = 84,
        SMART_ACTION_INVOKER_CAST = 85,
        SMART_ACTION_CROSS_CAST = 86,
        SMART_ACTION_CALL_RANDOM_TIMED_ACTIONLIST = 87,
        SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST = 88,
        SMART_ACTION_RANDOM_MOVE = 89,
        SMART_ACTION_SET_UNIT_FIELD_BYTES_1 = 90,
        SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1 = 91,
        SMART_ACTION_INTERRUPT_SPELL = 92,
        SMART_ACTION_SEND_GO_CUSTOM_ANIM = 93,
        SMART_ACTION_SET_DYNAMIC_FLAG = 94,
        SMART_ACTION_ADD_DYNAMIC_FLAG = 95,
        SMART_ACTION_REMOVE_DYNAMIC_FLAG = 96,
        SMART_ACTION_JUMP_TO_POS = 97,
        SMART_ACTION_SEND_GOSSIP_MENU = 98,
        SMART_ACTION_GO_SET_LOOT_STATE = 99,
        SMART_ACTION_SEND_TARGET_TO_TARGET = 100,
        SMART_ACTION_SET_HOME_POS = 101,
        SMART_ACTION_SET_HEALTH_REGEN = 102,
        SMART_ACTION_SET_ROOT = 103,
        SMART_ACTION_SET_GO_FLAG = 104,
        SMART_ACTION_ADD_GO_FLAG = 105,
        SMART_ACTION_REMOVE_GO_FLAG = 106,
        SMART_ACTION_SUMMON_CREATURE_GROUP = 107,
        SMART_ACTION_SET_POWER = 108,
        SMART_ACTION_ADD_POWER = 109,
        SMART_ACTION_REMOVE_POWER = 110,
    }

    public enum SmartEvent
    {
        SMART_EVENT_UPDATE_IC = 0,
        SMART_EVENT_UPDATE_OOC = 1,
        SMART_EVENT_HEALT_PCT = 2,
        SMART_EVENT_MANA_PCT = 3,
        SMART_EVENT_AGGRO = 4,
        SMART_EVENT_KILL = 5,
        SMART_EVENT_DEATH = 6,
        SMART_EVENT_EVADE = 7,
        SMART_EVENT_SPELLHIT = 8,
        SMART_EVENT_RANGE = 9,
        SMART_EVENT_OOC_LOS = 10,
        SMART_EVENT_RESPAWN = 11,
        SMART_EVENT_TARGET_HEALTH_PCT = 12,
        SMART_EVENT_TARGET_CASTING = 13,
        SMART_EVENT_FRIENDLY_HEALTH = 14,
        SMART_EVENT_FRIENDLY_IS_CC = 15,
        SMART_EVENT_FRIENDLY_MISSING_BUFF = 16,
        SMART_EVENT_SUMMONED_UNIT = 17,
        SMART_EVENT_TARGET_MANA_PCT = 18,
        SMART_EVENT_ACCEPTED_QUEST = 19,
        SMART_EVENT_REWARD_QUEST = 20,
        SMART_EVENT_REACHED_HOME = 21,
        SMART_EVENT_RECEIVE_EMOTE = 22,
        SMART_EVENT_HAS_AURA = 23,
        SMART_EVENT_TARGET_BUFFED = 24,
        SMART_EVENT_RESET = 25,
        SMART_EVENT_IC_LOS = 26,
        SMART_EVENT_PASSENGER_BOARDED = 27,
        SMART_EVENT_PASSENGER_REMOVED = 28,
        SMART_EVENT_CHARMED = 29,
        SMART_EVENT_CHARMED_TARGET = 30,
        SMART_EVENT_SPELLHIT_TARGET = 31,
        SMART_EVENT_DAMAGED = 32,
        SMART_EVENT_DAMAGED_TARGET = 33,
        SMART_EVENT_MOVEMENTINFORM = 34,
        SMART_EVENT_SUMMON_DESPAWNED = 35,
        SMART_EVENT_CORPSE_REMOVED = 36,
        SMART_EVENT_AI_INIT = 37,
        SMART_EVENT_DATA_SET = 38,
        SMART_EVENT_WAYPOINT_START = 39,
        SMART_EVENT_WAYPOINT_REACHED = 40,
        SMART_EVENT_TRANSPORT_ADDPLAYER_NYI = 41,
        SMART_EVENT_TRANSPORT_ADDCREATURE_NYI = 42,
        SMART_EVENT_TRANSPORT_REMOVE_PLAYER_NYI = 43,
        SMART_EVENT_TRANSPORT_RELOCATE_NYI = 44,
        SMART_EVENT_INSTANCE_PLAYER_ENTER_NYI = 45,
        SMART_EVENT_AREATRIGGER_ONTRIGGER = 46,
        SMART_EVENT_QUEST_ACCEPTED_NYI = 47,
        SMART_EVENT_QUEST_OBJ_COPLETETION_NYI = 48,
        SMART_EVENT_QUEST_COMPLETION_NYI = 49,
        SMART_EVENT_QUEST_REWARDED_NYI = 50,
        SMART_EVENT_QUEST_FAIL_NYI = 51,
        SMART_EVENT_TEXT_OVER = 52,
        SMART_EVENT_RECEIVE_HEAL = 53,
        SMART_EVENT_JUST_SUMMONED = 54,
        SMART_EVENT_WAYPOINT_PAUSED = 55,
        SMART_EVENT_WAYPOINT_RESUMED = 56,
        SMART_EVENT_WAYPOINT_STOPPED = 57,
        SMART_EVENT_WAYPOINT_ENDED = 58,
        SMART_EVENT_TIMED_EVENT_TRIGGERED = 59,
        SMART_EVENT_UPDATE = 60,
        SMART_EVENT_LINK = 61,
        SMART_EVENT_GOSSIP_SELECT = 62,
        SMART_EVENT_JUST_CREATED = 63,
        SMART_EVENT_GOSSIP_HELLO = 64,
        SMART_EVENT_FOLLOW_COMPLETED = 65,
        SMART_EVENT_DUMMY_EFFECT_NYI = 66,
        SMART_EVENT_IS_BEHIND_TARGET = 67,
        SMART_EVENT_GAME_EVENT_START = 68,
        SMART_EVENT_GAME_EVENT_END = 69,
        SMART_EVENT_GO_STATE_CHANGED = 70,
        SMART_EVENT_GO_EVENT_INFORM = 71,
        SMART_EVENT_ACTION_DONE = 72,
        SMART_EVENT_ON_SPELLCLICK = 73,
        SMART_EVENT_FRIENDLY_HEALTH_PCT = 74,
    }

    public enum SmartTargetType
    {
        SMART_TARGET_NONE = 0,
        SMART_TARGET_SELF = 1,
        SMART_TARGET_VICTIM = 2,
        SMART_TARGET_HOSTILE_SECOND_AGGRO = 3,
        SMART_TARGET_HOSTILE_LAST_AGGRO = 4,
        SMART_TARGET_HOSTILE_RANDOM = 5,
        SMART_TARGET_HOSTILE_RANDOM_NOT_TOP = 6,
        SMART_TARGET_ACTION_INVOKER = 7,
        SMART_TARGET_POSITION = 8,
        SMART_TARGET_CREATURE_RANGE = 9,
        SMART_TARGET_CREATURE_GUID = 10,
        SMART_TARGET_CREATURE_DISTANCE = 11,
        SMART_TARGET_STORED = 12,
        SMART_TARGET_GAMEOBJECT_RANGE = 13,
        SMART_TARGET_GAMEOBJECT_GUID = 14,
        SMART_TARGET_GAMEOBJECT_DISTANCE = 15,
        SMART_TARGET_INVOKER_PARTY = 16,
        SMART_TARGET_PLAYER_RANGE = 17,
        SMART_TARGET_PLAYER_DISTANCE = 18,
        SMART_TARGET_CLOSEST_CREATURE = 19,
        SMART_TARGET_CLOSEST_GAMEOBJECT = 20,
        SMART_TARGET_CLOSEST_PLAYER = 21,
        SMART_TARGET_ACTION_INVOKER_VEHICLE = 22,
        SMART_TARGET_OWNER_OR_SUMMONER = 23,
        SMART_TARGET_THREAT_LIST = 24,
        SMART_TARGET_CLOSEST_ENEMY = 25,
        SMART_TARGET_CLOSEST_FRIENDLY = 26,
    }

    public enum SmartReactState
    {
        REACT_PASSIVE = 0,
        REACT_DEFENSIVE = 1,
        REACT_AGGRESSIVE = 2
    }

    public enum SmartSourceType
    {
        SMART_SCRIPT_TYPE_CREATURE = 0,
        SMART_SCRIPT_TYPE_GAMEOBJECT = 1,
        SMART_SCRIPT_TYPE_AREATRIGGER = 2,
        SMART_SCRIPT_TYPE_EVENT_NYI = 3,
        SMART_SCRIPT_TYPE_GOSSIP_NYI = 4,
        SMART_SCRIPT_TYPE_QUEST_NYI = 5,
        SMART_SCRIPT_TYPE_SPELL_NYI = 6,
        SMART_SCRIPT_TYPE_TRANSPORT_NYI = 7,
        SMART_SCRIPT_TYPE_INSTANCE_NYI = 8,
        SMART_SCRIPT_TYPE_TIMED_ACTIONLIST = 9
    }

    public enum SmartSummonType
    {
        TEMPSUMMON_TIMED_OR_DEAD_DESPAWN = 1,
        TEMPSUMMON_TIMED_OR_CORPSE_DESPAWN = 2,
        TEMPSUMMON_TIMED_DESPAWN = 3,
        TEMPSUMMON_TIMED_DESPAWN_OUT_OF_COMBAT = 4,
        TEMPSUMMON_CORPSE_DESPAWN = 5,
        TEMPSUMMON_CORPSE_TIMED_DESPAWN = 6,
        TEMPSUMMON_DEAD_DESPAWN = 7,
        TEMPSUMMON_MANUAL_DESPAWN = 8
    }
}
