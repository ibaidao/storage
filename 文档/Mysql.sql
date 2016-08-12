/*
Navicat MySQL Data Transfer

Source Server         : jyzn
Source Server Version : 50713
Source Host           : localhost:3306
Source Database       : jyzn

Target Server Type    : MYSQL
Target Server Version : 50713
File Encoding         : 65001

Date: 2016-08-12 14:30:16
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `devices`
-- ----------------------------
DROP TABLE IF EXISTS `devices`;
CREATE TABLE `devices` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) DEFAULT NULL,
  `LocationID` int(11) DEFAULT NULL,
  `LocationXYZ` varchar(50) DEFAULT NULL,
  `Status` smallint(6) DEFAULT NULL,
  `IPAddress` varchar(50) DEFAULT NULL,
  `Manufacturer` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `OnlineDate` datetime DEFAULT NULL,
  `Remarks` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of devices
-- ----------------------------
INSERT INTO `devices` VALUES ('2', '112', '51', '500,1300,0', '3', '192.168.1.105', null, '2016-08-09 16:41:28', '0001-01-01 00:00:00', null);

-- ----------------------------
-- Table structure for `orders`
-- ----------------------------
DROP TABLE IF EXISTS `orders`;
CREATE TABLE `orders` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) DEFAULT NULL,
  `SkuList` varchar(50) DEFAULT NULL,
  `Priority` smallint(6) DEFAULT NULL,
  `productCount` smallint(6) DEFAULT NULL,
  `Status` smallint(6) DEFAULT NULL,
  `Picker` int(11) DEFAULT NULL,
  `StationID` int(11) DEFAULT NULL,
  `CreateTime` datetime DEFAULT NULL,
  `PickTime` datetime DEFAULT NULL,
  `Remarks` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of orders
-- ----------------------------

-- ----------------------------
-- Table structure for `products`
-- ----------------------------
DROP TABLE IF EXISTS `products`;
CREATE TABLE `products` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `SurfaceNum` smallint(6) DEFAULT NULL,
  `CellNum` smallint(6) DEFAULT NULL,
  `SkuID` int(11) DEFAULT NULL,
  `ShelfID` int(11) DEFAULT NULL,
  `ProductName` varchar(50) DEFAULT NULL,
  `ProductionDate` datetime DEFAULT NULL,
  `ExpireDate` datetime DEFAULT NULL,
  `Specification` varchar(50) DEFAULT NULL,
  `Weight` decimal(18,2) DEFAULT NULL,
  `Count` smallint(6) DEFAULT NULL,
  `UpShelfTime` datetime DEFAULT NULL,
  `DownShelfTime` datetime DEFAULT NULL,
  `Status` smallint(6) DEFAULT NULL,
  `Code` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of products
-- ----------------------------
INSERT INTO `products` VALUES ('1', '1', '2', '3', '2', '水杯10；红色300ml', '2015-07-01 00:00:00', '2016-12-31 00:00:00', '20*200*2000', '200.00', '87', '2016-07-01 10:51:50', '2016-08-11 15:20:33', '3', '444');
INSERT INTO `products` VALUES ('2', '1', '4', '4', '2', '水杯11；红色300ml', '2015-07-01 00:00:00', '2016-12-31 00:00:00', '20*200*2000', '200.00', '120', '2016-07-01 10:51:50', '2016-08-11 15:21:04', '3', '555');
INSERT INTO `products` VALUES ('3', '1', '5', '2', '3', '水杯2；红色300ml', '2015-07-01 00:00:00', '2016-12-31 00:00:00', '20*200*2000', '200.00', '180', '2016-07-01 10:51:50', '2016-08-11 11:58:31', '3', '666');
INSERT INTO `products` VALUES ('4', '1', '2', '5', '11', '水杯15；红色300ml', '2015-07-01 00:00:00', '2016-12-31 00:00:00', '20*200*2000', '200.00', '86', '2016-07-01 10:51:50', '2016-08-12 12:27:32', '3', '111');
INSERT INTO `products` VALUES ('5', '1', '1', '3', '12', '水杯10；红色300ml', '2015-07-01 00:00:00', '2016-12-31 00:00:00', '20*200*2000', '200.00', '87', '2016-07-01 10:51:50', '2016-08-12 14:07:54', '3', '222');
INSERT INTO `products` VALUES ('6', '1', '5', '4', '12', '水杯11；红色300ml', '2015-07-01 00:00:00', '2016-12-31 00:00:00', '20*200*2000', '200.00', '82', '2016-07-01 10:51:50', '2016-08-12 14:08:40', '3', '333');
INSERT INTO `products` VALUES ('7', '1', '3', '6', '16', '水杯41；红色300ml', '2015-07-01 00:00:00', '2016-12-31 00:00:00', '20*200*2000', '200.00', '95', '2016-07-01 10:51:50', '2016-08-12 11:28:25', '3', '432');
INSERT INTO `products` VALUES ('8', '1', '4', '7', '42', '水杯31；红色300ml', '2015-07-01 00:00:00', '2016-12-31 00:00:00', '20*200*2000', '200.00', '96', '2016-07-01 10:51:50', '2016-08-12 11:32:28', '3', '777');
INSERT INTO `products` VALUES ('9', '1', '5', '8', '42', '水杯33；红色300ml', '2015-07-01 00:00:00', '2016-12-31 00:00:00', '20*200*2000', '200.00', '96', '2016-07-01 10:51:50', '2016-08-12 10:55:05', '3', '888');
INSERT INTO `products` VALUES ('10', '1', '6', '12', '32', '水杯66；红色300ml', '2015-07-01 00:00:00', '2016-12-31 00:00:00', '20*200*2000', '200.00', '99', '2016-07-01 10:51:50', '2016-08-12 10:48:27', '3', '999');
INSERT INTO `products` VALUES ('11', '1', '4', '8', '16', '水杯33；红色300ml', '2015-07-01 00:00:00', '2016-12-31 00:00:00', '20*200*2000', '200.00', '50', '2016-07-01 10:51:50', '0001-01-01 00:00:00', '0', '123');

-- ----------------------------
-- Table structure for `realorders`
-- ----------------------------
DROP TABLE IF EXISTS `realorders`;
CREATE TABLE `realorders` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `OrderID` int(11) DEFAULT NULL,
  `ProductCount` smallint(6) DEFAULT NULL,
  `SkuList` varchar(50) DEFAULT NULL,
  `StaffID` int(11) DEFAULT NULL,
  `StationID` int(11) DEFAULT NULL,
  `PickDevices` varchar(50) DEFAULT NULL,
  `PickProducts` varchar(50) DEFAULT NULL,
  `PickProductCount` smallint(6) DEFAULT NULL,
  `Status` smallint(6) DEFAULT NULL,
  `PickRemark` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `OrderID` (`OrderID`)
) ENGINE=InnoDB AUTO_INCREMENT=420 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of realorders
-- ----------------------------

-- ----------------------------
-- Table structure for `realproducts`
-- ----------------------------
DROP TABLE IF EXISTS `realproducts`;
CREATE TABLE `realproducts` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `StationID` int(11) DEFAULT NULL,
  `OrderID` int(11) DEFAULT NULL,
  `SkuID` int(11) DEFAULT NULL,
  `ProductCount` smallint(6) DEFAULT NULL,
  `PickProductCount` smallint(6) DEFAULT NULL,
  `Status` smallint(6) DEFAULT NULL,
  `LastTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `StationID` (`StationID`)
) ENGINE=InnoDB AUTO_INCREMENT=609 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of realproducts
-- ----------------------------

-- ----------------------------
-- Table structure for `realshelf`
-- ----------------------------
DROP TABLE IF EXISTS `realshelf`;
CREATE TABLE `realshelf` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ShelfID` int(11) DEFAULT NULL,
  `DeviceID` int(11) DEFAULT NULL,
  `ProductCount` smallint(6) DEFAULT NULL,
  `ProductID` varchar(50) DEFAULT NULL,
  `SkuID` varchar(50) DEFAULT NULL,
  `OrderID` varchar(50) DEFAULT NULL,
  `StationID` varchar(50) DEFAULT NULL,
  `GetOrderTime` datetime DEFAULT NULL,
  `GetShelfTime` datetime DEFAULT NULL,
  `StartTransTime` datetime DEFAULT NULL,
  `SentShelfTime` datetime DEFAULT NULL,
  `FinishPickTime` datetime DEFAULT NULL,
  `ReturnShelfTime` datetime DEFAULT NULL,
  `Status` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `ShelfID` (`ShelfID`,`DeviceID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of realshelf
-- ----------------------------

-- ----------------------------
-- Table structure for `shelf`
-- ----------------------------
DROP TABLE IF EXISTS `shelf`;
CREATE TABLE `shelf` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) DEFAULT NULL,
  `LocationID` int(11) DEFAULT NULL,
  `LocHistory` varchar(50) DEFAULT NULL,
  `Layer` smallint(6) DEFAULT NULL,
  `Surface` smallint(6) DEFAULT NULL,
  `Address` varchar(50) DEFAULT NULL,
  `Type` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of shelf
-- ----------------------------
INSERT INTO `shelf` VALUES ('1', '02A211', '1', '1', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('2', '02A211', '2', '2', '4', '2', '01020201;02030302', '1');
INSERT INTO `shelf` VALUES ('3', '02A211', '3', '3', '4', '2', '01020201;02030301', '1');
INSERT INTO `shelf` VALUES ('4', '02A211', '4', '4', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('5', '02A211', '5', '5', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('6', '02A211', '6', '6', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('7', '02A211', '7', '7', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('8', '02A211', '8', '8', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('9', '02A211', '9', '9', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('10', '02A211', '10', '10', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('11', '02A211', '11', '11', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('12', '02A211', '12', '12', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('13', '02A211', '13', '13', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('14', '02A211', '14', '14', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('15', '02A211', '15', '15', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('16', '02A211', '16', '16', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('17', '02A211', '17', '17', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('18', '02A211', '18', '18', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('19', '02A211', '19', '19', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('20', '02A211', '20', '20', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('21', '02A211', '21', '21', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('22', '02A211', '22', '22', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('23', '02A211', '23', '23', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('24', '02A211', '24', '24', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('25', '02A211', '25', '25', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('26', '02A211', '26', '26', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('27', '02A211', '27', '27', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('28', '02A211', '28', '28', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('29', '02A211', '29', '29', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('30', '02A211', '30', '30', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('31', '02A211', '31', '31', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('32', '02A211', '32', '32', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('33', '02A211', '33', '33', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('34', '02A211', '34', '34', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('35', '02A211', '35', '35', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('36', '02A211', '36', '36', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('37', '02A211', '37', '37', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('38', '02A211', '38', '38', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('39', '02A211', '39', '39', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('40', '02A211', '40', '40', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('41', '02A211', '41', '41', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('42', '02A211', '42', '42', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('43', '02A211', '43', '43', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('44', '02A211', '44', '44', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('45', '02A211', '45', '45', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('46', '02A211', '46', '46', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('47', '02A211', '47', '47', '4', '2', '01020201;01020301', '1');
INSERT INTO `shelf` VALUES ('48', '02A211', '48', '48', '4', '2', '01020201;01020301', '1');

-- ----------------------------
-- Table structure for `skuinfo`
-- ----------------------------
DROP TABLE IF EXISTS `skuinfo`;
CREATE TABLE `skuinfo` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `Count` int(11) DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `Color` varchar(50) DEFAULT NULL,
  `Size` varchar(50) DEFAULT NULL,
  `Weight` decimal(18,2) DEFAULT NULL,
  `Remarks` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of skuinfo
-- ----------------------------
INSERT INTO `skuinfo` VALUES ('2', '水杯2', '10', '300ml', '红色', '20*200*2000', '200.00', null);
INSERT INTO `skuinfo` VALUES ('3', '水杯10', '10', '300ml', '红色', '20*200*2000', '200.00', null);
INSERT INTO `skuinfo` VALUES ('4', '水杯11', '20', '300ml', '红色', '20*200*2000', '200.00', null);
INSERT INTO `skuinfo` VALUES ('5', '水杯15', '20', '300ml', '红色', '20*200*2000', '200.00', null);
INSERT INTO `skuinfo` VALUES ('6', '水杯41', '20', '300ml', '红色', '20*200*2000', '200.00', null);
INSERT INTO `skuinfo` VALUES ('7', '水杯31', '10', '300ml', '红色', '20*200*2000', '200.00', null);
INSERT INTO `skuinfo` VALUES ('8', '水杯33', '20', '300ml', '红色', '20*200*2000', '200.00', null);
INSERT INTO `skuinfo` VALUES ('12', '水杯66', '210', '301ml', '红色', '20*200*2000', '0.00', null);

-- ----------------------------
-- Table structure for `staff`
-- ----------------------------
DROP TABLE IF EXISTS `staff`;
CREATE TABLE `staff` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `Sex` tinyint(1) DEFAULT NULL,
  `Age` smallint(6) DEFAULT NULL,
  `Phone` varchar(50) DEFAULT NULL,
  `Address` varchar(50) DEFAULT NULL,
  `Job` varchar(50) DEFAULT NULL,
  `Auth` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Phone` (`Phone`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of staff
-- ----------------------------
INSERT INTO `staff` VALUES ('1', 'Suoxd1', '1', '21', '150150150151', '深圳南山1', 'Software1', '11101');
INSERT INTO `staff` VALUES ('2', 'Suoxd2', '1', '21', '150150150152', '深圳南山1', 'Software1', '11101');

-- ----------------------------
-- Table structure for `station`
-- ----------------------------
DROP TABLE IF EXISTS `station`;
CREATE TABLE `station` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) DEFAULT NULL,
  `StoreID` smallint(6) DEFAULT NULL,
  `IPAddress` varchar(50) DEFAULT NULL,
  `LocationID` int(11) DEFAULT NULL,
  `Location` varchar(50) DEFAULT NULL,
  `Status` smallint(6) DEFAULT NULL,
  `Type` smallint(6) DEFAULT NULL,
  `Remarks` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of station
-- ----------------------------
INSERT INTO `station` VALUES ('1', 'PickStation1', '0', '192.168.1.106', '49', '500,1300,0', '9', '2', null);
INSERT INTO `station` VALUES ('2', 'PickStation2', '0', '192.168.1.107', '50', '1200,1300,0', '9', '2', null);
INSERT INTO `station` VALUES ('3', 'Charge1', '0', null, '51', '200,300,0', '0', '4', null);
INSERT INTO `station` VALUES ('4', 'Charge2', '0', null, '52', '1400,300,0', '0', '4', null);
INSERT INTO `station` VALUES ('5', 'Charge3', '0', null, '53', '200,600,0', '0', '4', null);
INSERT INTO `station` VALUES ('6', 'Charge4', '0', null, '54', '1400,600,0', '0', '4', null);
INSERT INTO `station` VALUES ('7', 'Charge5', '0', null, '55', '200,900,0', '0', '4', null);
INSERT INTO `station` VALUES ('8', 'Charge6', '0', null, '56', '1400,900,0', '0', '4', null);
INSERT INTO `station` VALUES ('9', 'Charge7', '0', null, '57', '200,1100,0', '0', '4', null);
INSERT INTO `station` VALUES ('10', 'Charge8', '0', null, '58', '1400,1100,0', '0', '4', null);
INSERT INTO `station` VALUES ('11', 'Charge9', '0', null, '59', '200,1200,0', '0', '4', null);
INSERT INTO `station` VALUES ('12', 'Charge10', '0', null, '60', '1400,1200,0', '0', '4', null);
INSERT INTO `station` VALUES ('18', '101', '0', null, '101', '1300,1200,0', '1', '5', '修改类型：原类型5，改为5');

-- ----------------------------
-- Table structure for `storepaths`
-- ----------------------------
DROP TABLE IF EXISTS `storepaths`;
CREATE TABLE `storepaths` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `StoreID` smallint(6) DEFAULT NULL,
  `OnePoint` int(11) DEFAULT NULL,
  `TwoPoint` int(11) DEFAULT NULL,
  `Weight` smallint(6) DEFAULT NULL,
  `Type` smallint(6) DEFAULT NULL,
  `Status` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=135 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of storepaths
-- ----------------------------
INSERT INTO `storepaths` VALUES ('2', '1', '51', '61', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('3', '1', '61', '62', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('4', '1', '62', '63', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('5', '1', '63', '64', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('6', '1', '64', '85', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('7', '1', '85', '86', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('8', '1', '86', '87', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('9', '1', '87', '65', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('10', '1', '65', '66', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('11', '1', '66', '67', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('12', '1', '67', '68', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('13', '1', '68', '52', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('14', '1', '53', '69', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('15', '1', '69', '70', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('16', '1', '70', '71', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('17', '1', '71', '72', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('18', '1', '72', '88', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('19', '1', '88', '89', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('20', '1', '89', '90', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('21', '1', '90', '73', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('22', '1', '73', '74', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('23', '1', '74', '75', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('24', '1', '75', '76', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('25', '1', '76', '54', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('26', '1', '55', '77', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('27', '1', '77', '78', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('28', '1', '78', '79', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('29', '1', '79', '80', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('30', '1', '80', '91', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('31', '1', '91', '92', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('32', '1', '92', '93', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('33', '1', '93', '81', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('34', '1', '81', '82', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('35', '1', '82', '83', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('36', '1', '83', '84', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('37', '1', '84', '56', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('38', '1', '57', '94', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('39', '1', '94', '95', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('41', '1', '96', '97', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('42', '1', '97', '58', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('43', '1', '59', '98', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('44', '1', '98', '99', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('45', '1', '99', '100', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('46', '1', '100', '101', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('47', '1', '101', '60', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('48', '1', '85', '88', '1', '7', '9');
INSERT INTO `storepaths` VALUES ('49', '1', '88', '91', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('50', '1', '86', '89', '1', '7', '9');
INSERT INTO `storepaths` VALUES ('51', '1', '89', '92', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('52', '1', '87', '90', '1', '7', '9');
INSERT INTO `storepaths` VALUES ('53', '1', '90', '93', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('54', '1', '98', '102', '1', '8', '0');
INSERT INTO `storepaths` VALUES ('55', '1', '102', '49', '1', '8', '0');
INSERT INTO `storepaths` VALUES ('56', '1', '49', '103', '1', '8', '0');
INSERT INTO `storepaths` VALUES ('57', '1', '103', '99', '1', '8', '0');
INSERT INTO `storepaths` VALUES ('58', '1', '100', '104', '1', '8', '0');
INSERT INTO `storepaths` VALUES ('59', '1', '104', '50', '1', '8', '0');
INSERT INTO `storepaths` VALUES ('60', '1', '50', '105', '1', '8', '0');
INSERT INTO `storepaths` VALUES ('61', '1', '105', '101', '1', '8', '0');
INSERT INTO `storepaths` VALUES ('62', '1', '2', '61', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('63', '1', '61', '10', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('64', '1', '3', '62', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('65', '1', '62', '11', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('66', '1', '4', '63', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('67', '1', '63', '12', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('68', '1', '5', '64', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('69', '1', '64', '13', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('70', '1', '6', '65', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('71', '1', '65', '14', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('72', '1', '7', '66', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('73', '1', '66', '15', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('74', '1', '8', '67', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('75', '1', '67', '16', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('76', '1', '9', '68', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('77', '1', '68', '17', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('78', '1', '18', '69', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('79', '1', '69', '25', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('80', '1', '19', '70', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('81', '1', '70', '26', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('82', '1', '20', '71', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('83', '1', '71', '27', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('84', '1', '21', '72', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('85', '1', '72', '28', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('86', '1', '22', '73', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('87', '1', '73', '29', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('88', '1', '23', '74', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('89', '1', '74', '30', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('90', '1', '24', '75', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('91', '1', '75', '31', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('92', '1', '1', '76', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('93', '1', '76', '32', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('94', '1', '33', '77', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('95', '1', '77', '41', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('96', '1', '34', '78', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('97', '1', '78', '42', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('98', '1', '35', '79', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('99', '1', '79', '43', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('100', '1', '36', '80', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('101', '1', '80', '44', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('102', '1', '37', '81', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('103', '1', '81', '45', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('104', '1', '38', '82', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('105', '1', '82', '46', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('106', '1', '39', '83', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('107', '1', '83', '47', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('108', '1', '40', '84', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('109', '1', '84', '48', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('110', '1', '94', '98', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('111', '1', '95', '99', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('112', '1', '96', '100', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('113', '1', '97', '101', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('114', '1', '91', '107', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('115', '1', '92', '108', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('116', '1', '93', '109', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('122', '1', '95', '107', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('123', '1', '107', '108', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('124', '1', '108', '109', '1', '7', '0');
INSERT INTO `storepaths` VALUES ('125', '1', '109', '96', '1', '7', '0');

-- ----------------------------
-- Table structure for `storepoints`
-- ----------------------------
DROP TABLE IF EXISTS `storepoints`;
CREATE TABLE `storepoints` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `StoreID` smallint(6) DEFAULT NULL,
  `Point` varchar(50) DEFAULT NULL,
  `Type` smallint(6) DEFAULT NULL,
  `Status` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=129 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of storepoints
-- ----------------------------
INSERT INTO `storepoints` VALUES ('1', 'D12', '1', '1300,500,0', '1', '0');
INSERT INTO `storepoints` VALUES ('2', 'A2', '1', '300,200,0', '1', '0');
INSERT INTO `storepoints` VALUES ('3', 'A3', '1', '400,200,0', '1', '0');
INSERT INTO `storepoints` VALUES ('4', 'A4', '1', '500,200,0', '1', '0');
INSERT INTO `storepoints` VALUES ('5', 'A5', '1', '600,200,0', '1', '0');
INSERT INTO `storepoints` VALUES ('6', 'A9', '1', '1000,200,0', '1', '0');
INSERT INTO `storepoints` VALUES ('7', 'A10', '1', '1100,200,0', '1', '0');
INSERT INTO `storepoints` VALUES ('8', 'A11', '1', '1200,200,0', '1', '0');
INSERT INTO `storepoints` VALUES ('9', 'A12', '1', '1300,200,0', '1', '0');
INSERT INTO `storepoints` VALUES ('10', 'C2', '1', '300,400,0', '1', '0');
INSERT INTO `storepoints` VALUES ('11', 'C3', '1', '400,400,0', '1', '0');
INSERT INTO `storepoints` VALUES ('12', 'C4', '1', '500,400,0', '1', '0');
INSERT INTO `storepoints` VALUES ('13', 'C5', '1', '600,400,0', '1', '0');
INSERT INTO `storepoints` VALUES ('14', 'C9', '1', '1000,400,0', '1', '0');
INSERT INTO `storepoints` VALUES ('15', 'C10', '1', '1100,400,0', '1', '0');
INSERT INTO `storepoints` VALUES ('16', 'C11', '1', '1200,400,0', '1', '0');
INSERT INTO `storepoints` VALUES ('17', 'C12', '1', '1300,400,0', '1', '0');
INSERT INTO `storepoints` VALUES ('18', 'D2', '1', '300,500,0', '1', '0');
INSERT INTO `storepoints` VALUES ('19', 'D3', '1', '400,500,0', '1', '0');
INSERT INTO `storepoints` VALUES ('20', 'D4', '1', '500,500,0', '1', '0');
INSERT INTO `storepoints` VALUES ('21', 'D5', '1', '600,500,0', '1', '0');
INSERT INTO `storepoints` VALUES ('22', 'D9', '1', '1000,500,0', '1', '0');
INSERT INTO `storepoints` VALUES ('23', 'D10', '1', '1100,500,0', '1', '0');
INSERT INTO `storepoints` VALUES ('24', 'D11', '1', '1200,500,0', '1', '0');
INSERT INTO `storepoints` VALUES ('25', 'F2', '1', '300,700,0', '1', '0');
INSERT INTO `storepoints` VALUES ('26', 'F3', '1', '400,700,0', '1', '0');
INSERT INTO `storepoints` VALUES ('27', 'F4', '1', '500,700,0', '1', '0');
INSERT INTO `storepoints` VALUES ('28', 'F5', '1', '600,700,0', '1', '0');
INSERT INTO `storepoints` VALUES ('29', 'F9', '1', '1000,700,0', '1', '0');
INSERT INTO `storepoints` VALUES ('30', 'F10', '1', '1100,700,0', '1', '0');
INSERT INTO `storepoints` VALUES ('31', 'F11', '1', '1200,700,0', '1', '0');
INSERT INTO `storepoints` VALUES ('32', 'F12', '1', '1300,700,0', '1', '0');
INSERT INTO `storepoints` VALUES ('33', 'G2', '1', '300,800,0', '1', '0');
INSERT INTO `storepoints` VALUES ('34', 'G3', '1', '400,800,0', '1', '0');
INSERT INTO `storepoints` VALUES ('35', 'G4', '1', '500,800,0', '1', '0');
INSERT INTO `storepoints` VALUES ('36', 'G5', '1', '600,800,0', '1', '0');
INSERT INTO `storepoints` VALUES ('37', 'G9', '1', '1000,800,0', '1', '0');
INSERT INTO `storepoints` VALUES ('38', 'G10', '1', '1100,800,0', '1', '0');
INSERT INTO `storepoints` VALUES ('39', 'G11', '1', '1200,800,0', '1', '0');
INSERT INTO `storepoints` VALUES ('40', 'G12', '1', '1300,800,0', '1', '0');
INSERT INTO `storepoints` VALUES ('41', 'I2', '1', '300,1000,0', '1', '0');
INSERT INTO `storepoints` VALUES ('42', 'I3', '1', '400,1000,0', '1', '0');
INSERT INTO `storepoints` VALUES ('43', 'I4', '1', '500,1000,0', '1', '0');
INSERT INTO `storepoints` VALUES ('44', 'I5', '1', '600,1000,0', '1', '0');
INSERT INTO `storepoints` VALUES ('45', 'I9', '1', '1000,1000,0', '1', '0');
INSERT INTO `storepoints` VALUES ('46', 'I10', '1', '1100,1000,0', '1', '0');
INSERT INTO `storepoints` VALUES ('47', 'I11', '1', '1200,1000,0', '1', '0');
INSERT INTO `storepoints` VALUES ('48', 'I12', '1', '1300,1000,0', '1', '0');
INSERT INTO `storepoints` VALUES ('49', 'L4', '1', '500,1300,0', '2', '8');
INSERT INTO `storepoints` VALUES ('50', 'L11', '1', '1200,1300,0', '2', '8');
INSERT INTO `storepoints` VALUES ('51', 'B1', '1', '200,300,0', '4', '0');
INSERT INTO `storepoints` VALUES ('52', 'B13', '1', '1400,300,0', '4', '0');
INSERT INTO `storepoints` VALUES ('53', 'E1', '1', '200,600,0', '4', '0');
INSERT INTO `storepoints` VALUES ('54', 'E13', '1', '1400,600,0', '4', '0');
INSERT INTO `storepoints` VALUES ('55', 'H1', '1', '200,900,0', '4', '0');
INSERT INTO `storepoints` VALUES ('56', 'H13', '1', '1400,900,0', '4', '0');
INSERT INTO `storepoints` VALUES ('57', 'J1', '1', '200,1100,0', '4', '0');
INSERT INTO `storepoints` VALUES ('58', 'J13', '1', '1400,1100,0', '4', '0');
INSERT INTO `storepoints` VALUES ('59', 'K1', '1', '200,1200,0', '4', '0');
INSERT INTO `storepoints` VALUES ('60', 'K13', '1', '1400,1200,0', '4', '0');
INSERT INTO `storepoints` VALUES ('61', 'B2', '1', '300,300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('62', 'B3', '1', '400,300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('63', 'B4', '1', '500,300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('64', 'B5', '1', '600,300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('65', 'B9', '1', '1000,300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('66', 'B10', '1', '1100,300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('67', 'B11', '1', '1200,300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('68', 'B12', '1', '1300,300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('69', 'E2', '1', '300,600,0', '0', '0');
INSERT INTO `storepoints` VALUES ('70', 'E3', '1', '400,600,0', '0', '0');
INSERT INTO `storepoints` VALUES ('71', 'E4', '1', '500,600,0', '0', '0');
INSERT INTO `storepoints` VALUES ('72', 'E5', '1', '600,600,0', '0', '0');
INSERT INTO `storepoints` VALUES ('73', 'E9', '1', '1000,600,0', '0', '0');
INSERT INTO `storepoints` VALUES ('74', 'E10', '1', '1100,600,0', '0', '0');
INSERT INTO `storepoints` VALUES ('75', 'E11', '1', '1200,600,0', '0', '0');
INSERT INTO `storepoints` VALUES ('76', 'E12', '1', '1300,600,0', '0', '0');
INSERT INTO `storepoints` VALUES ('77', 'H2', '1', '300,900,0', '0', '0');
INSERT INTO `storepoints` VALUES ('78', 'H3', '1', '400,900,0', '0', '0');
INSERT INTO `storepoints` VALUES ('79', 'H4', '1', '500,900,0', '0', '0');
INSERT INTO `storepoints` VALUES ('80', 'H5', '1', '600,900,0', '0', '0');
INSERT INTO `storepoints` VALUES ('81', 'H9', '1', '1000,900,0', '0', '0');
INSERT INTO `storepoints` VALUES ('82', 'H10', '1', '1100,900,0', '0', '0');
INSERT INTO `storepoints` VALUES ('83', 'H11', '1', '1200,900,0', '0', '0');
INSERT INTO `storepoints` VALUES ('84', 'H12', '1', '1300,900,0', '0', '0');
INSERT INTO `storepoints` VALUES ('85', 'B6', '1', '700,300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('86', 'B7', '1', '800,300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('87', 'B8', '1', '900,300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('88', 'E6', '1', '700,600,0', '0', '0');
INSERT INTO `storepoints` VALUES ('89', 'E7', '1', '800,600,0', '0', '0');
INSERT INTO `storepoints` VALUES ('90', 'E8', '1', '900,600,0', '0', '0');
INSERT INTO `storepoints` VALUES ('91', 'H6', '1', '700,900,0', '0', '0');
INSERT INTO `storepoints` VALUES ('92', 'H7', '1', '800,900,0', '0', '0');
INSERT INTO `storepoints` VALUES ('93', 'H8', '1', '900,900,0', '0', '0');
INSERT INTO `storepoints` VALUES ('94', 'J2', '1', '300,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('95', 'J5', '1', '600,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('96', 'J9', '1', '1000,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('97', 'J12', '1', '1300,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('98', 'K2', '1', '300,1200,0', '0', '0');
INSERT INTO `storepoints` VALUES ('99', 'K5', '1', '600,1200,0', '0', '0');
INSERT INTO `storepoints` VALUES ('100', 'K9', '1', '1000,1200,0', '0', '0');
INSERT INTO `storepoints` VALUES ('101', 'K12', '1', '1300,1200,0', '5', '0');
INSERT INTO `storepoints` VALUES ('102', 'L2', '1', '300,1300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('103', 'L5', '1', '600,1300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('104', 'L9', '1', '1000,1300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('105', 'L12', '1', '1300,1300,0', '0', '0');
INSERT INTO `storepoints` VALUES ('107', 'J6', '1', '700,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('108', 'J7', '1', '800,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('109', 'J8', '1', '900,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('111', 'J6', '1', '700,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('112', 'J7', '1', '800,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('113', 'J8', '1', '900,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('115', 'J6', '1', '700,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('116', 'J7', '1', '800,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('117', 'J8', '1', '900,1100,0', '0', '0');
INSERT INTO `storepoints` VALUES ('119', 'UT', '1', '1,1,1', '0', '0');
