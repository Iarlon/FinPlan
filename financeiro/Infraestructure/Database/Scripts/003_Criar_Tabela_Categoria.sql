CREATE TABLE categoria (
	id BIGSERIAL PRIMARY KEY,
	descricao VARCHAR(100) NOT NULL UNIQUE,
	tipo_movimentacao_id BIGINT NOT NULL,

	CONSTRAINT fk_categoria_tipo
	FOREIGN KEY (tipo_movimentacao_id)
	REFERENCES tipo_movimentacao(id)
);