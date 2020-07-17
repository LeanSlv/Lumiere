function include(url) {
  var script = document.createElement('script');  
  script.src = url;
  document.getElementsByTagName('head')[0].appendChild(script);
}

include("/authValid.js");


describe("Ввод email при регистрации", function() {

  it("правильный ввод email: remirboy@yandex.ru", function() {
    assert.equal(validateEmail('remirboy@yandex.ru'),true);
  });

  it("неправильный ввод email: remirboy", function() {
    assert.equal(validateEmail('remirboy'),false);
  });

  it("неправильный ввод email: remirboy@", function() {
    assert.equal(validateEmail('remirboy@'),false);
  });

  it("неправильный ввод email: remirboy@yandex", function() {
    assert.equal(validateEmail('remirboy@yandex'),false);
  });

   it("неправильный ввод email: remirboy@yandex.", function() {
    assert.equal(validateEmail('remirboy@yandex.'),false);
  });

});

describe("Ввод пароля при регистрации", function() {

  it("правильный ввод пароля: reMir23", function() {
    assert.equal(validatePass('reMir23'),true);
  });

  it("неправильный ввод пароля: remir23", function() {
    assert.equal(validatePass('remir23'),false);
  });

  it("неправильный ввод пароля: reMir", function() {
    assert.equal(validatePass('reMir'),false);
  });

  it("неправильный ввод пароля: remir", function() {
    assert.equal(validatePass('remir'),false);
  });
});

describe("Совпадение паролей при регистрации", function() {

  it("пароли совпали", function() {
    assert.equal(validateRePass('reMir23','reMir23'),true);
  });

  it("пароли не совпали", function() {
    assert.equal(validateRePass('reMir24','reMir23'),false);
  });

  it("пароль не соответствует требованиям", function() {
    assert.equal(validateRePass('remir','remir'),false);
  });
});

describe("Проверка имени+фамилии", function() {

  it("имя правильно введено", function() {
    assert.equal(validateName('Ремир'),true);
  });

  it("имя введено не на русском", function() {
    assert.equal(validateName('Remir'),false);
  });

  it("смесь русскго и английского недопустимо", function() {
    assert.equal(validateName('реDир'),false);
  });
});

