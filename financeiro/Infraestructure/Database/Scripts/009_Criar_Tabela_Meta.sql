CREATE TABLE meta (
	id BIGSERIAL PRIMARY KEY,
	usuario_id BIGINT NOT NULL,
	data_final TIMESTAMP NOT NULL,
	orcamento_id BIGINT NOT NULL,
	descricao VARCHAR(255) NOT NULL,
	valor_aporte NUMERIC(18,2) NOT NULL,
	valor_objetivo NUMERIC(18,2) NOT NULL,
	periodicidade VARCHAR(50) NOT NULL,
	status_id int NOT NULL,
	CONSTRAINT ck_meta_valor
		CHECK (valor_objetivo > 0),

	CONSTRAINT ck_aporte_valor
	CHECK (valor_aporte > 0),
	
	CONSTRAINT FK_Meta_Status 
	FOREIGN KEY (status_id) REFERENCES status_meta(id),
	CONSTRAINT FK_Usuario
	FOREIGN KEY (usuario_id) REFERENCES usuario(id),
	CONSTRAINT FK_Orcamento
	FOREIGN KEY (orcamento_id) REFERENCES orcamento(id)
);