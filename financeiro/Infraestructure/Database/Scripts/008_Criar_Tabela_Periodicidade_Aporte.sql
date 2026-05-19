CREATE TABLE periodicidade_aporte (
	id BIGSERIAL PRIMARY KEY,
	descricao VARCHAR(100) NOT NULL UNIQUE
);

INSERT INTO periodicidade_aporte (id, descricao) VALUES
(1,'Mensal'),
(2,'Bimestral'),
(3,'Trimestral'),
(4,'Anual');