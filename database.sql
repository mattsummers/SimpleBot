
-- ----------------------------
-- Table structure for config
-- ----------------------------
DROP TABLE IF EXISTS `config`;
CREATE TABLE `config` (
  `ConfigId` int(11) NOT NULL AUTO_INCREMENT,
  `KeyName` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT '',
  `KeyValue` text COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`ConfigId`),
  KEY `IX_Key` (`KeyName`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;


-- ----------------------------
-- Table structure for editlog
-- ----------------------------
DROP TABLE IF EXISTS `editlog`;
CREATE TABLE `editlog` (
  `LogId` int(11) NOT NULL AUTO_INCREMENT,
  `EntryId` int(11) NOT NULL DEFAULT '0',
  `DateEditedUtc` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `MemberId` int(11) NOT NULL DEFAULT '0',
  `Phrase` varchar(50) CHARACTER SET utf8mb4 NOT NULL DEFAULT '',
  `Response` text CHARACTER SET utf8mb4 NOT NULL,
  `StartsWith` tinyint(1) NOT NULL DEFAULT '0',
  `Hidden` tinyint(1) NOT NULL DEFAULT '0',
  `AllowRepeat` tinyint(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`LogId`)
) ENGINE=InnoDB AUTO_INCREMENT=1488 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;


-- ----------------------------
-- Table structure for entries
-- ----------------------------
DROP TABLE IF EXISTS `entries`;
CREATE TABLE `entries` (
  `EntryId` int(11) NOT NULL AUTO_INCREMENT,
  `Phrase` varchar(50) NOT NULL DEFAULT '',
  `Response` text NOT NULL,
  `StartsWith` tinyint(1) NOT NULL DEFAULT '0',
  `Hidden` tinyint(1) NOT NULL DEFAULT '0',
  `MemberId` int(11) NOT NULL DEFAULT '0',
  `AllowRepeat` tinyint(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`EntryId`),
  KEY `IX_Key` (`Phrase`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=618 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of entries
-- ----------------------------
INSERT INTO `entries` VALUES ('1', 'Hello', 'Hi {username}!!', '1', '0', '1', '0');

-- ----------------------------
-- Table structure for entryowners
-- ----------------------------
DROP TABLE IF EXISTS `entryowners`;
CREATE TABLE `entryowners` (
  `EntryOwnerId` int(11) NOT NULL AUTO_INCREMENT,
  `EntryId` int(11) NOT NULL DEFAULT '0',
  `MemberId` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`EntryOwnerId`),
  KEY `IX_EntryId` (`EntryId`) USING BTREE,
  KEY `IX_MemberId` (`MemberId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=391 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of entryowners
-- ----------------------------
INSERT INTO `entryowners` VALUES ('1', '1', '1');


-- ----------------------------
-- Table structure for newsentries
-- ----------------------------
DROP TABLE IF EXISTS `newsentries`;
CREATE TABLE `newsentries` (
  `NewsId` int(11) NOT NULL AUTO_INCREMENT,
  `DatePostedUtc` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Content` text COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`NewsId`),
  KEY `IX_dateposted` (`DatePostedUtc`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ----------------------------
-- Records of newsentries
-- ----------------------------


-- ----------------------------
-- Table structure for randominsults
-- ----------------------------
DROP TABLE IF EXISTS `randominsults`;
CREATE TABLE `randominsults` (
  `RandomInsultId` int(11) NOT NULL AUTO_INCREMENT,
  `Insult` varchar(50) NOT NULL DEFAULT '',
  PRIMARY KEY (`RandomInsultId`)
) ENGINE=InnoDB AUTO_INCREMENT=102 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of randominsults
-- ----------------------------
INSERT INTO `randominsults` VALUES ('1', 'jerk');


-- ----------------------------
-- Table structure for tinyurls
-- ----------------------------
DROP TABLE IF EXISTS `tinyurls`;
CREATE TABLE `tinyurls` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Url` varchar(3000) CHARACTER SET utf8mb4 NOT NULL DEFAULT '',
  `ShortUrl` varchar(200) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_ShortUrl` (`ShortUrl`)
) ENGINE=InnoDB AUTO_INCREMENT=293 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;


-- ----------------------------
-- Table structure for userroles
-- ----------------------------
DROP TABLE IF EXISTS `userroles`;
CREATE TABLE `userroles` (
  `RoleId` int(11) NOT NULL AUTO_INCREMENT,
  `MemberId` int(11) NOT NULL DEFAULT '0',
  `Role` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`RoleId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ----------------------------
-- Records of userroles
-- ----------------------------
INSERT INTO `userroles` VALUES ('1', '1', '100');
INSERT INTO `userroles` VALUES ('2', '0', '0');

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `MemberId` int(11) NOT NULL AUTO_INCREMENT,
  `Email` varchar(50) NOT NULL DEFAULT '',
  `Password` varchar(200) NOT NULL DEFAULT '',
  `Salt` varchar(200) NOT NULL DEFAULT '',
  `Approved` bit(1) NOT NULL DEFAULT b'0',
  `Name` varchar(200) NOT NULL DEFAULT '',
  PRIMARY KEY (`MemberId`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES ('1', 'you@domain.com', 'set email and reset password to log in', '', '\0', 'Admin');
SET FOREIGN_KEY_CHECKS=1;
