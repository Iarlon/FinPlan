CREATE TABLE categoria (
	id BIGSERIAL PRIMARY KEY,
	descricao VARCHAR(100) NOT NULL,
	tipo_movimentacao_id BIGINT NOT NULL,

	CONSTRAINT fk_categoria_tipo
	FOREIGN KEY (tipo_movimentacao_id)
	REFERENCES tipo_movimentacao(id)
);

INSERT INTO categoria (id, descricao, tipo_movimentacao_id) VALUES
    (1, 'Alimentacao', 2),
    (2, 'Moradia', 2),
    (3, 'Transporte', 2),
    (4, 'Lazer', 2),
    (5, 'Saude', 2),
    (6, 'Educacao', 2),
    (7, 'Investimentos', 2),
    (100, 'Salario', 1),
    (101, 'Bonus', 1),
    (102, 'Comissao', 1),
    (103, 'HoraExtra', 1),
    (104, 'Freelance', 1),
    (107, 'Transferencias', 1),
    (108, 'Vendas', 1),
    (109, 'Servicos', 1),
    (998, 'Outros', 2),
    (999, 'Outros', 1);