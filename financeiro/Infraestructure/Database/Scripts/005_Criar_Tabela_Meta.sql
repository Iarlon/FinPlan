CREATE TABLE meta (
	id BIGSERIAL PRIMARY KEY,
	usuario_id INT NOT NULL,
	data_final TIMESTAMP NOT NULL,
	orcamento_id INT NOT NULL,
	descricao VARCHAR(255) NOT NULL,
	valor_aporte NUMERIC(18,2) NOT NULL,
	valor_objetivo NUMERIC(18,2) NOT NULL,
	periodicidade VARCHAR(50) NOT NULL,
	status_id int NOT NULL,
	
	status_id INT NOT NULL,
	CONSTRAINT ck_meta_valor
		CHECK (valor > 0),
	
	CONSTRAINT FK_Meta_Status 
	FOREIGN KEY (status_id) REFERENCES meta_status(id),
	CONSTRAINT FK_Usuario
	FOREIGN KEY (usuario_id) REFERENCES usuario(id),
	CONSTRAINT FK_Orcamento
	FOREIGN KEY (orcamento_id) REFERENCES orcamento(id)
);