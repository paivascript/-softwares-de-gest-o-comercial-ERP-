create database mercadoseuze;

use mercadoseuze;

CREATE TABLE usuario(
    id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    login VARCHAR(50) UNIQUE NOT NULL,
    senha VARCHAR(100) NOT NULL,
    nivel VARCHAR(20),
    dataCadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE cliente(
    id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    cpf VARCHAR(14) UNIQUE,
    telefone VARCHAR(20),
    email VARCHAR(100),
    estadoCivil VARCHAR(50),
    sexo VARCHAR(20),
    endereco VARCHAR(200),
    status BOOLEAN DEFAULT TRUE,
    dataCadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE produto(
    id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    descricao VARCHAR(255),
    preco DECIMAL(10,2) NOT NULL,
    estoque INT NOT NULL,
    categoria VARCHAR(100),
    status BOOLEAN DEFAULT TRUE,
    dataCadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE venda(
    id INT PRIMARY KEY AUTO_INCREMENT,
    idCliente INT,
    idUsuario INT,
    dataVenda DATETIME DEFAULT CURRENT_TIMESTAMP,
    valorTotal DECIMAL(10,2),

    FOREIGN KEY(idCliente) REFERENCES cliente(id),
    FOREIGN KEY(idUsuario) REFERENCES usuario(id)
);


CREATE TABLE item_venda(
    id INT PRIMARY KEY AUTO_INCREMENT,
    idVenda INT,
    idProduto INT,
    quantidade INT,
    precoUnitario DECIMAL(10,2),
    subtotal DECIMAL(10,2),

    FOREIGN KEY(idVenda) REFERENCES venda(id),
    FOREIGN KEY(idProduto) REFERENCES produto(id)
);

CREATE TABLE log_sistema(
    id INT PRIMARY KEY AUTO_INCREMENT,
    usuario VARCHAR(100),
    acao VARCHAR(255),
    dataAcao DATETIME DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO usuario(nome,login,senha,nivel)
VALUES('Administrador','admin','1234','ADMIN');



