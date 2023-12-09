export class ClassNameUtil {
  className = [];

  add = (
    className,
    checkableValue = true
  ) => {
    if (className && checkableValue) {
      this.className.push(className);
    }
  };

  remove = (
    className,
    checkableValue = true
  ) => {
    if (className && checkableValue) {
      const index = this.className.indexOf(className);
      this.className.splice(index);
    }
  };

  getClassName = () => {
    return this.className.join(' ');
  }
}
