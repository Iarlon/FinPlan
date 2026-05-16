CREATE TABLE movimentacao (
    id BIGSERIAL PRIMARY KEY,
    usuario_id INT NOT NULL,
    categoria_id INT NOT NULL,
    tipo_id INT NOT NULL,
    valor NUMERIC(18,2) NOT NULL,
    data_geracao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_movimentacao TIMESTAMP NOT NULL,
    descricao VARCHAR(255),
    tag VARCHAR(100),

    CONSTRAINT ck_movimentacao_valor
        CHECK (valor > 0),
    
    CONSTRAINT FK_Movimentacao_Categoria 
    FOREIGN KEY (categoria_id) REFERENCES categoria(id),
    CONSTRAINT FK_Movimentacao_Usuario
    FOREIGN KEY (usuario_id) REFERENCES usuario(id),
    CONSTRAINT FK_Movimentacao_Tipo
    FOREIGN KEY (tipo_id) REFERENCES tipo_movimentacao(id)
);
