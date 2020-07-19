function include(url) {
  var script = document.createElement('script');  
  script.src = url;
  document.getElementsByTagName('head')[0].appendChild(script);
}

include("/authValid.js");


describe("email при авторизации", function() {

  it("корректный email: sasha.kochedykov@mail.ru", function() {
    assert.equal(validateEmail('sasha.kochedykov@mail.ru'),true);
  });

  it("некорректный email: sasha.kochedykov@", function() {
    assert.equal(validateEmail('sasha.kochedykov@'),false);
  });

  it("некорректный email: sasha.kochedykov", function() {
    assert.equal(validateEmail('sasha.kochedykov'),false);
  });

  it("некорректный email: sasha.kochedykov@mail", function() {
    assert.equal(validateEmail('sasha.kochedykov@mail'),false);
  });

   it("некорректный email: @mail.ru", function() {
    assert.equal(validateEmail('@mail.ru'),false);
  });

});




