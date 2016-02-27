-- -----------------------------------------------------
-- Schema INFMAN01_1
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS INFMAN01_1;

-- -----------------------------------------------------
-- Schema INFMAN01_1
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS INFMAN01_1 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
-- -----------------------------------------------------
-- Schema INFMAN01_1
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Table `INFMAN01_1`.`garagedata`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `INFMAN01_1`.`garagedata` ;

CREATE TABLE IF NOT EXISTS `INFMAN01_1`.`garagedata` (
	`identifier` 		VARCHAR(45) 	NOT NULL ,
	`description` 		VARCHAR(255) 	NOT NULL ,
	`open` 				BOOLEAN			NOT NULL ,
	`full` 				BOOLEAN 		NOT NULL ,
	`parking capacity` 	INT		 		NOT NULL ,
	`vacant spaces` 	INT 			NOT NULL ,
	`last updated` 		datetime 		NOT NULL ,
  PRIMARY KEY (`identifier`)  );

