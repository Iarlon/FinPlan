CREATE TABLE orcamento (
	id BIGSERIAL PRIMARY KEY,
	usuario_id BIGINT UNIQUE NOT NULL,
	saldo_conta DECIMAL(18, 2) NOT NULL,

	CONSTRAINT fk_usuario_id
		FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);