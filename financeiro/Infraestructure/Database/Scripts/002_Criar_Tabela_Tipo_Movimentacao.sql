CREATE TABLE tipo_movimentacao (
    id BIGSERIAL PRIMARY KEY,
    descricao VARCHAR(100) NOT NULL UNIQUE
);

INSERT INTO tipo_movimentacao (id, descricao)
VALUES
(1,'Receita'),
(2,'Despesa');