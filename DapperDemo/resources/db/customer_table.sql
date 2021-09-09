use csharp_demo;
drop table if exists `customer`;
CREATE TABLE `customer`
(
    `id`    int NOT NULL AUTO_INCREMENT,
    `name`  varchar(255) DEFAULT NULL,
    `email` int         DEFAULT NULL,
    unique index idx_uni_name (name),
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4;