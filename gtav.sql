/*
Navicat MySQL Data Transfer

Source Server         : dedie
Source Server Version : 50505
Source Host           : 164.132.200.96:3406
Source Database       : gtav

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2017-04-27 23:05:11
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `user`
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `name` varchar(20) NOT NULL,
  `money` int(20) NOT NULL,
  `skin` text NOT NULL,
  `ip` varchar(255) NOT NULL,
  `group` int(2) NOT NULL DEFAULT '0',
  `bank` int(10) NOT NULL,
  `jailed` tinyint(1) NOT NULL DEFAULT '0',
  `inventory` text NOT NULL,
  `position` text NOT NULL,
  `characters` text NOT NULL,
  `adminrank` int(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of user
-- ----------------------------

-- ----------------------------
-- Table structure for `vehicles`
-- ----------------------------
DROP TABLE IF EXISTS `vehicles`;
CREATE TABLE `vehicles` (
  `id` int(6) NOT NULL AUTO_INCREMENT,
  `side` varchar(16) NOT NULL,
  `classname` varchar(64) NOT NULL,
  `type` varchar(16) NOT NULL,
  `pid` varchar(17) NOT NULL,
  `plate` varchar(8) NOT NULL,
  `color` int(20) NOT NULL,
  `inventory` varchar(64) NOT NULL,
  `fuel` double NOT NULL DEFAULT '1',
  `rotation` varchar(64) NOT NULL,
  `position` varchar(256) NOT NULL,
  `active` int(1) NOT NULL,
  `insert_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_time` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`,`active`),
  KEY `side` (`side`),
  KEY `pid` (`pid`),
  KEY `type` (`type`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of vehicles
-- ----------------------------
